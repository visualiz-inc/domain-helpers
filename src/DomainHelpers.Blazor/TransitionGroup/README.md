トランジションの開始と終了を定義するのに便利なシンプルなコンポーネントを公開します。
このコンポーネントは、それ自体でスタイルをアニメートするわけではありません。その代わりに、トランジションステージを公開し、クラスとグループ要素を管理し、DOMを有用な方法で操作することで、実際の視覚的トランジションの実装をより簡単にします。

トランジションコンポーネントは、あるコンポーネントからの移行を記述することができます。
は、シンプルな宣言型APIを使用して、時間の経過とともに別の状態に変化させることができます。
最も一般的には、コンポーネントのマウントとアンマウントをアニメーション化するために使用されます、
が、インプレースの遷移状態も記述するために使用することができます。

デフォルトでは、Transitionコンポーネントは、レンダリングするコンポーネントの動作を変更せず、コンポーネントの「入力」「終了」状態のみを追跡します。
これらの状態に意味と効果を与えるのは、あなた次第です。例えば、コンポーネントが「入る」「出る」ときにスタイルを追加することができます：

トランジションには、主に4つの状態があります：

* ```entering```
* ```entered```
* ```exiting```
* ```exited```

Transitionを継承し、BuildRenderTreeメソッドとしてrazorテンプレートをオーバーライドします。


```razor

// GrowTransition.razor
@using BlazorTransitionGroup

@inherits Transition

<div style="@ActualStyle" class="@Class">
    @ChildContent?.Invoke(TransitionState)
</div>

@code {
    string ActualStyle => $"opacity: {Opacity};transform:scale({Size});transition:opacity {Duration / 2}ms ease-in-out,transform {Duration}ms ease-in-out;{Style}";

    string Opacity => TransitionState switch {
        TransitionState.Entering or TransitionState.Entered => "1",
        _ => "0",
    };

    string Size => TransitionState switch {
        TransitionState.Entering or TransitionState.Entered => "1",
        _ => "0",
    };

    double Duration => TransitionState switch {
        TransitionState.Entering or TransitionState.Entered => DurationEnter,
        _ => DurationExit,
    };

    string HeightStyle => TransitionState switch {
        TransitionState.Entering or TransitionState.Entered => "100%",
        _ => "0%",
    };

    [Parameter]
    public string Height { get; set; } = "auto";

    [Parameter]
    public string Width { get; set; } = "auto";

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Class { get; set; }
}

```

### Public API Reference

| Name                | Type            | Category            | Default                | Description                                                 |
|---------------------|-----------------|---------------------|------------------------|-------------------------------------------------------------|
| ChildContent        | RenderFragment? | Component Attribute | null                   | The Render fragment for child content.                      |
| TransitionBegan     | EventCallback   | Component Attribute | ---                    | The event callback that fired when Transition is Began.     |
| TransitionCompleted | EventCallback   | Component Attribute | ---                    | The event callback that fired when Transition is Completed. |
| Delay               | int             | Component Attribute | 0                      | The milliseconds of deley time that animation begin.        |
| DurationEnter       | int             | Component Attribute | 400                    | The Duration of entering animation.                         |
| DurationExit        | int             | Component Attribute | 400                    | The Duration of exiting animation.                          |
| In                  | bool?           | Component Attribute | null                   | Is the animation enabled.                                   |
| IsAnimating         | bool            | Property            | false                  | Whether it is currently animated.                           |
| TransitionState     | TransitionState | Property            | TransitionState.Exited | The current transition state.                               |
| Dispose             | void Dispose()  | Method              | ---                    | Dispose current context.                                    |

### TransitionGroup

トランジションコンポーネントのセットを管理する `<TransitionGroup>` コンポーネントです。
(`<トランジション>`)をリストで表示します。

トランジションコンポーネントと同様に、`<TransitionGroup>`は、トランジションを管理するためのステートマシンとなります。
部品の取り付け、取り外しが時間の経過とともに変化すること。

以下の例を考えてみましょう。
TodoListに項目が削除されたり、追加されたりすると
は `<TransitionGroup>` によって自動的にトグルされます。

なお、`<TransitionGroup>`はアニメーションの動作を定義するものではありません！
リストアイテムがどのようにアニメーションするかは、個々のトランジションコンポーネントに任されています。
つまり、異なるリストアイテム間でアニメーションを混ぜて使うことができます。

トランジショングループの構成要素には、一意の `@key` フィールドが必要です。
アニメーションさせる `Transition` コンポーネントは `TransitionGroup` の直下に配置する必要があります。
`div`や他のコンポーネントで囲むと動作しません。

OK
```razor
<TransitionGroup>
    @foreach(var item in items) {
       <HogeTransition @key="item" />
    }
</TransitionGroup>
```

Failed
```razor
<TransitionGroup>
    @foreach(var item in items) { 
        <div @key="item">
            <HogeTransition @key="item" />
        </div>
    }
</TransitionGroup>
```

[Sample code in Demo is here](./samples/BlazorTransitionGroup.Samples/Demo/TransitionDemo.razor)

```razor

@using BlazorTransitionGroup

<TransitionGroup>
    @foreach (var (i, text, id) in _items) {
        @if (i % 2 is 0) {
            <GrowTransition @key="@($"{text}-{id}")" Context="state">
                <div class="item d-flex p-3 align-items-center shadow mt-3 rounded-3 bg-white">
                    <button class="btn btn-danger" @onclick="@(() => Remove((i, text, id)))">
                        <i class="oi oi-trash" />
                    </button>
                    <div class="p-1 mx-3" style="width:100px;">@state</div>
                    <div class="p-1 mx-3">@text</div>
                </div>
            </GrowTransition>
        }
        else {
            <SlideTransition @key="@($"{text}-{id}")">
                <div class="item d-flex p-3 align-items-center shadow mt-3 rounded-3 bg-white">
                    <button class="btn btn-danger" @onclick="@(() => Remove((i, text, id)))">
                        <i class="oi oi-trash" />
                    </button>
                    <div class="p-1 mx-3" style="width:100px;"></div>
                    <div class="p-1 mx-3">@text</div>
                </div>
            </SlideTransition>
        }
    }
</TransitionGroup>

<div class="d-flex mt-4">
    <input @bind-value="_text " />
    <button class="btn btn-primary" @onclick="Add"> ADD</button>
</div>

@code {
    string _text = "";
    int _i = 3;

    List<(int Index, string Text, Guid Key)> _items = new() {
        (0, "item 1", Guid.NewGuid()),
        (1, "item 2", Guid.NewGuid()),
        (2, "item 3", Guid.NewGuid()),
    };

    void Add() {
        if (string.IsNullOrWhiteSpace(_text)) {
            return;
        }

        _items.Add((_i++, _text, Guid.NewGuid()));
        _text = "";
    }

    void Remove((int, string, Guid) text) {
        _items.Remove(text);
    }
}

```

### Public API Reference
| Name          | Type               | Description                                                         |  
| ------------- | ------------------ | ------------------------------------------------------------------- |
| ChildContent  | RenderFragment?    | The render fragment for ChildContent.                               |

### TransitionBase

トランジションを実装するもう一つの方法は、`TransitionBase`を継承することです。
トランジションはコンポジションで実装することができます。
RenderFragmentのコンテキストプロバイダは、トランジションステートを提供します。
ここでは、Transitionコンポーネントの使用例を示します。
Transition AttributeにKeyを指定するのを忘れないようにしましょう。

```razor

@using BlazorTransitionGroup

@inherits TransitionBase

<Transition Key="Key" Context="transitionState">
    <div style="@(GetActualStyle(transitionState))" class="@Class">
        @ChildContent
    </div>
</Transition>

@code {
    string GetActualStyle(TransitionState state) {
        var (x, opacity) = (GetX(state), GetOpacity(state));
        return $"opacity: {opacity};transform:translateX({x});transition:opacity {Duration / 2}ms ease-in-out,transform {Duration}ms ease-in-out;{Style}";
    }

    string GetOpacity(TransitionState state) {
        return state switch {
            TransitionState.Entering or TransitionState.Entered => "1",
            _ => "0",
        };
    }

    double Duration => 800;

    string GetX(TransitionState state) {
        return state switch {
            TransitionState.Entering or TransitionState.Entered => "0%",
            _ => "-50%",
        };
    }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

```
