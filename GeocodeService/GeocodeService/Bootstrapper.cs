using Nancy;
using Nancy.TinyIoc;
using Nancy.Bootstrapper;
//using GeocodeService.Models;

namespace GeocodeService
{
   
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        //protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            //Enable CORS
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")  //CORS
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
        }
    }
}
