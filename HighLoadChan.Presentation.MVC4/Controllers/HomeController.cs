using System.Web.Mvc;

namespace HighLoadChan.Presentation.MVC4.Controllers
{
    using System.Net;

    using HighLoadChan.Core;
    using HighLoadChan.Presentation.MVC4.Models;

    public class HomeController : Controller
    {
        private readonly CommandFaccade commandFaccade;
        private readonly IReadModel readModel;

        public HomeController(CommandFaccade commandFaccade, IReadModel readModel)
        {
            this.commandFaccade = commandFaccade;
            this.readModel = readModel;
        }

        public ActionResult Index()
        {
            var model = this.readModel.GetBoardPage(0, "");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddThread(ThreadWebModel model)
        {
            if (ModelState.IsValid)
            {
                this.commandFaccade.CreateThread(new Thread()
                {
                    BoardName = "b",
                    Name = model.Name
                });

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult AddPost(PostWebModel model)
        {
            if (ModelState.IsValid)
            {
                this.commandFaccade.AddPost(new Post()
                {
                    ThreadId = model.ThreadId,
                    Author = model.Author,
                    Content = model.Content
                });

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }

}
