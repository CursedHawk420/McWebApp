using Highgeek.McWebApp.Common.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OpenApi.Highgeek.LuckPermsApi.Model;

namespace Highgeek.McWebApp.BlazorServer.ComponentBases
{
    public class LanguageBase : ComponentBase, IDisposable
    {
        [Inject]
        public ILocalizer l {  get; set; }

        public bool _disposed = false;

        public MarkdownPipeline markdownPipeline;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            l.LocaleRefreshRequested += RefreshLanguageAsync;
        }

        public async void RefreshLanguageAsync()
        {
            // InvokeAsync is inherited, it syncs the call back to the render thread
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        public MarkupString GetMarkdown(string wordKey)
        {
            if (markdownPipeline is null)
            {
                markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            }
            return (MarkupString)Markdown.ToHtml(l[wordKey], markdownPipeline);
        }


        void IDisposable.Dispose()
        {
            // Dispose of unmanaged resources.
            if (!_disposed)
            {
                Dispose(true);
                l.LocaleRefreshRequested -= RefreshLanguageAsync;
            }
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {

        }
    }
}
