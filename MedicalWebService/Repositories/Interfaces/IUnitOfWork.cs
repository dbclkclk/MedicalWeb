using MedicalWebService.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalWebService.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Client> ClientRepository { get; set; }
        GenericRepository<Patient> PatientRepository { get; set; }

        GenericRepository<IdentityRole> RoleRepository { get; set; }

        GenericRepository<IdentityUserRole> UserRoleRepository { get; set; }

        GenericRepository<AuditLog> AuditLogRepository { get; set; }
        void Save();
    }
}
