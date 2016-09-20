using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    [Table("ApplicationClients")]
    public class ApplicationClient
    {
        [Key]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string RedirectUris { get; set; }
        public string PostLogoutRedirectUris { get; set; }

        public string ClientSecrets { get; set; }
        public string GrantTypes { get; set; }
        public string AllowedScopes { get; set; }


        public List<string> ListRedirectUris
        {
            get { return RedirectUris.Split(',').ToList(); }
        }

        public List<string> ListClientSecrets
        {
            get { return ClientSecrets.Split(',').ToList(); }
        }

        public List<string> ListGrantType
        {
            get { return GrantTypes.Split(',').ToList(); }
        }

        public List<string> ListAllowedScopes
        {
            get { return AllowedScopes.Split(',').ToList(); }
        }

        public List<string> ListPostLogoutRedirectUris
        {
            get { return PostLogoutRedirectUris.Split(',').ToList(); }
        }
    }

}
