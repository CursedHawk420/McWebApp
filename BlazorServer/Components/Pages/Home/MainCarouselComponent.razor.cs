using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using MudBlazor;

namespace Highgeek.McWebApp.BlazorServer.Components.Pages.Home
{
    public partial class MainCarouselComponent
    {
        private bool arrows = true;
        private bool bullets = true;
        private bool enableSwipeGesture = true;
        private bool autocycle = false;
        private Transition transition = Transition.Slide;

        private List<CarouselContent> carouselContent = new List<CarouselContent>();

        protected override void OnInitialized()
        {
            var data = _cmsContext.CarouselContents.OrderBy(c => c.Order);
            carouselContent = data.ToList();
            carouselContent.RemoveAll(s => s.Visible == false);
        }

        public Task IndexChanged(int pos)
        {
            _logger.LogInformation("Carousel index changed: " + pos.ToString());
            return Task.CompletedTask;
        }


        void IDisposable.Dispose()
        {
            _cmsContext.Dispose();
        }
    }
}