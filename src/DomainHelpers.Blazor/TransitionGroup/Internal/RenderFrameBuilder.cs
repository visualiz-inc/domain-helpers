﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Runtime.InteropServices;

namespace DomainHelpers.Blazor.TransitionGroup.Internal;
#pragma warning disable BL0006

enum RenderFrame {
    OpenElement,
    CloseElement,
    OpenComponent,
    CloseComponent,
    Attribute,
    Text,
    ElementReferenceCapture,
    ComponentReferenceCapture,
}

[StructLayout(LayoutKind.Explicit, Pack = 4)]
struct Frame {
    [FieldOffset(0)] RenderFrame _renderFrame;

    // ref
    [FieldOffset(8)] Action<ElementReference> _lementReferenceCaptureAction;
    [FieldOffset(8)] Action<object> _componentReferenceCaptureAction;

    // component and element
    [FieldOffset(8)] object? _key;

    // component
    [FieldOffset(16)] Type _type;
    [FieldOffset(24)] bool _isAppendKey;

    // element
    [FieldOffset(16)] string _name;

    // attribute
    [FieldOffset(24)] RenderTreeFrame _renderTreeFrame;

    // text content
    [FieldOffset(8)] string _text;

    public RenderFrame RenderFrame {
        get => _renderFrame;
        set => _renderFrame = value;
    }

    public object? Key {
        get => _key;
        set => _key = value;
    }

    public Type Type {
        get => _type;
        set => _type = value;
    }

    public bool IsAppendKey {
        get => _isAppendKey;
        set => _isAppendKey = value;
    }

    public string Name {
        get => _name;
        set => _name = value;
    }

    public string Text {
        get => _text;
        set => _text = value;
    }

    public RenderTreeFrame RenderTreeFrame {
        get => _renderTreeFrame;
        set => _renderTreeFrame = value;
    }

    public Action<ElementReference> ElementReferenceCaptureAction {
        get => _lementReferenceCaptureAction;
        set => _lementReferenceCaptureAction = value;
    }

    public Action<object> ComponentReferenceCaptureAction {
        get => _componentReferenceCaptureAction;
        set => _componentReferenceCaptureAction = value;
    }
};

class RenderFrameBuilder(object key) {
    readonly List<Frame> _frames = [];

    public object Key { get; } = key;

    public bool IsAnimationElement { get; set; }

    public void Add(Frame frame) {
        _frames.Add(frame);
    }

    public void RenderChild(ref int sequence, RenderTreeBuilder builder) {
        foreach (var frame in _frames) {
            switch (frame.RenderFrame) {
                case RenderFrame.OpenElement: {
                        builder.OpenElement(sequence++, frame.Name);
                        if (frame.Key != null) {
                            builder.SetKey(frame.Name);
                        }
                        break;
                    }
                case RenderFrame.OpenComponent: {
                        builder.OpenComponent(sequence++, frame.Type);
                        if (frame.Key != null) {
                            builder.SetKey(frame.Key);
                        }

                        if (frame.IsAppendKey) {
                            builder.AddAttribute(sequence++, "Key", frame.Key);
                        }
                        break;
                    }
                case RenderFrame.Attribute: {
                        builder.AddAttribute(sequence++, frame.RenderTreeFrame);
                        break;
                    }
                case RenderFrame.Text: {
                        builder.AddContent(sequence++, frame.Text);
                        break;
                    }
                case RenderFrame.ElementReferenceCapture: {
                        builder.AddElementReferenceCapture(sequence++, frame.ElementReferenceCaptureAction);
                        break;
                    }
                case RenderFrame.ComponentReferenceCapture: {
                        builder.AddComponentReferenceCapture(sequence++, frame.ComponentReferenceCaptureAction);
                        break;
                    }
                case RenderFrame.CloseComponent: {
                        builder.CloseComponent();
                        break;
                    }
                case RenderFrame.CloseElement: {
                        builder.CloseElement();
                        break;
                    }
            }
        }
    }
}