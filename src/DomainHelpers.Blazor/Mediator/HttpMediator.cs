using DomainHelpers.Commons;
using MediatR;
using System.Text.Json;

namespace DomainHelpers.Blazor.Mediator;

/// <summary>
/// サーバーにリクエストを送信するメディエータです．
/// </summary>
public class HttpMediator {
    private readonly HttpClient _httpClient;

    /// <summary>
    /// <see cref="HttpMediator"/> クラスのインスタンスを初期化します.
    /// </summary>
    /// <param name="httpClient">HTTPクライアント．</param>
    public HttpMediator(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /// <summary>
    /// HTTPでリクエストをサーバーに送信します．
    /// </summary>
    /// <typeparam name="TResponse">結果のタイプ．</typeparam>
    /// <param name="request">リクエスト．</param>
    /// <returns>リクエスト結果．</returns>
    /// <exception cref="ApplicationException" >
    /// ハンドルされていないサーバーエラーが発生し，リクエストに失敗した際にスローされます．
    /// </exception>
    /// <exception cref="GeneralException{FailedResponse}" >
    /// Payload:<see cref="FailedResponse"/>
    /// リクエストに失敗した際にスローします．
    /// </exception>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request) {
        var response = await SendRequest(request);
        if (typeof(TResponse) == typeof(Unit)) {
            return default!;
        }

        var result = await response.ReadFromJsonAsync<TResponse>();
        return result!;
    }

    private async Task<HttpContent> SendRequest<T>(IRequest<T> request) {
        var requestType = request.GetType();
        var options = new JsonSerializerOptions { PropertyNamingPolicy = null, WriteIndented = true };
        var content = JsonContent.Create(request, requestType, options: options);
        var httpResponse = await _httpClient.PostAsync($"/mediator/{requestType.Name}", content);
        if (httpResponse.IsSuccessStatusCode is false) {
            try {
                var problem = await httpResponse.Content.ReadFromJsonAsync<FailedResponse?>();
                if (problem is not null) {
                    throw GeneralException.WithMessage(
                        problem,
                        problem.Title,
                        problem.Title
                    );
                }

                throw new ApplicationException(httpResponse.ReasonPhrase);
            }
            catch (GeneralException<FailedResponse> ex) {
                throw new RequestException(
                    ErrorType.Unknown,
                    httpResponse?.ReasonPhrase ?? "Request error",
                    "リクエストに失敗しました",
                    ex.Payload,
                    ex
                );
            }
            catch (Exception ex) {
                throw new RequestException(
                    ErrorType.Unknown,
                    httpResponse?.ReasonPhrase ?? "Request error",
                    "リクエストに失敗しました",
                    null,
                    ex
                );
            }
        }

        return httpResponse.Content;
    }
}

public enum ErrorType {
    NotConnected,
    NotFound,
    BadRequest,
    Unauthorized,
    Forbidden,
    InternalServerError,
    Unknown
}

public class RequestException : GeneralException<ErrorType> {
    public FailedResponse? FailedResponse { get; private set; }
    public RequestException(
        ErrorType type,
        string message,
        string? displayMessage = null,
        FailedResponse? failedResponse = null,
        Exception? ex = null
    ) : base(
        type,
        message,
        displayMessage,
        default,
        ex
    ) {
        FailedResponse = failedResponse;
    }
}