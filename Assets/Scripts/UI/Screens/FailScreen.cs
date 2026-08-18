using System;
using Larje.Core;
using Larje.Core.Services.UI;
using ProjectConstants;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : UIScreen
{
    [SerializeField] private Button retryButton;

    private Args _args;

    protected override void OnBeforeOpen(UIObject.Args args)
    {
        DIContainer.InjectTo(this);

        if (args is Args failArgs)
        {
            _args = failArgs;
        }

        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    protected override void OnBeforeClose()
    {
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        _args.OnRetry?.Invoke();
    }

    public class Args : UIScreen.Args
    {
        public readonly Action OnRetry;

        public Args(Action onRetry) : base(UIScreenType.Fail)
        {
            OnRetry = onRetry;
        }
    }
}
