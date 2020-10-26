﻿namespace PnP.Core.Model.SharePoint
{
    [GraphType]
    internal partial class TermLocalizedDescription : BaseDataModel<ITermLocalizedDescription>, ITermLocalizedDescription
    {
        public string LanguageTag { get => GetValue<string>(); set => SetValue(value); }

        public string Description { get => GetValue<string>(); set => SetValue(value); }

        [KeyProperty(nameof(LanguageTag))]
        public override object Key { get => this.LanguageTag; set => this.LanguageTag = value.ToString(); }
    }
}
