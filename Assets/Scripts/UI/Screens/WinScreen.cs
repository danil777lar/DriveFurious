using System;
using Larje.Core;
using Larje.Core.Services.UI;
using ProjectConstants;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : UIScreen
{
    [SerializeField] private Button nextButton;

    private Args _args;

    protected override void OnBeforeOpen(UIObject.Args args)
    {
        DIContainer.InjectTo(this);

        if (args is Args winArgs)
        {
            _args = winArgs;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    protected override void OnBeforeClose()
    {
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        _args.OnNext?.Invoke();
    }

    public class Args : UIScreen.Args
    {
        public readonly Action OnNext;

        public Args(Action onNext) : base(UIScreenType.Win)
        {
            OnNext = onNext;
        }
    }
}
