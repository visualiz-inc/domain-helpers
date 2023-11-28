using DomainHelpers.Commons;
using MediatR;
using System.Text.Json;

namespace DomainHelpers.Blazor.Mediator;
/// <summary>
/// サーバーにリクエストを送信するメディエータです．
/// </summary>
/// <remarks>
/// <see cref="HttpMediator"/> クラスのインスタンスを初期化します.
/// </remarks>
/// <param name="httpClient">HTTPクライアント．</param>
public class HttpMediator(HttpClient httpClient) : IMediator {
    private readonly HttpClient _httpClient = httpClient;

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification {
        throw new NotImplementedException();
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

        var json = await response.ReadAsStringAsync();
        if (json is "" or null) {
            return default!;
        }

        var result = JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result!;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest {
        throw new NotImplementedException();
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
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

public class RequestException(
    ErrorType type,
    string message,
    string? displayMessage = null,
    FailedResponse? failedResponse = null,
    Exception? ex = null
) : GeneralException<ErrorType>(
    type,
    message,
    displayMessage,
    default,
    ex
) {
    public FailedResponse? FailedResponse { get; private set; } = failedResponse;
}