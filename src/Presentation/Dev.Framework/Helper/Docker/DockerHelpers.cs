using Dev.Framework.Helper.Configuration;
using Dev.Framework.Model;
using Microsoft.Extensions.Configuration;

namespace Dev.Framework.Helper.Docker
{
    public class DockerHelpers
    {
        public static void UpdateCaCertificates()
        {
            "update-ca-certificates".Bash();
        }

        /// <summary>
        /// {DockerConfiguration : {UpdateCaCertificate:true}}
        /// </summary>
        /// <param name="configuration"></param>
        public static void ApplyDockerConfiguration(IConfiguration configuration)
        {
            var dockerConfiguration = configuration.GetSection(nameof(DockerConfiguration)).Get<DockerConfiguration>();

            if (dockerConfiguration != null && dockerConfiguration.UpdateCaCertificate)
            {
                UpdateCaCertificates();
            }
        }
    }
}