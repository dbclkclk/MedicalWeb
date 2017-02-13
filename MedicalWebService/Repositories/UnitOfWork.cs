using MedicalWebService.Models;
using MedicalWebService.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedicalWebService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext context;
        private GenericRepository<Client> clientRepository;
        private GenericRepository<Patient> patientRepository;
        private GenericRepository<IdentityRole> roleRepository;
        private GenericRepository<IdentityUserRole> userRoleRepository;
        private GenericRepository<AuditLog> auditLogRepository;
        public UnitOfWork (DbContext ctx)
        {
            this.context = ctx;

        }
        public GenericRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new GenericRepository<Client>(context);
                }
                return clientRepository;
            }
            set
            {

                this.clientRepository = value;
                
            }
        }

        public GenericRepository<Patient> PatientRepository
        {
            get
            {

                if (this.patientRepository == null)
                {
                    this.patientRepository = new GenericRepository<Patient>(context);
                }
                return patientRepository;
            }
            set
            {

                this.patientRepository = value;

            }
        }

        public GenericRepository<IdentityUserRole> UserRoleRepository
        {
            get
            {

                if (this.userRoleRepository == null)
                {
                    this.userRoleRepository = new GenericRepository<IdentityUserRole>(context);
                }
                return userRoleRepository;
            }
            set
            {

                this.userRoleRepository = value;

            }
        }
        public GenericRepository<IdentityRole> RoleRepository
        {
            get
            {

                if (this.roleRepository == null)
                {
                    this.roleRepository = new GenericRepository<IdentityRole>(context);
                }
                return roleRepository;
            }
            set
            {

                this.roleRepository = value;

            }
        }


        public GenericRepository<AuditLog> AuditLogRepository
        {
            get
            {

                if (this.auditLogRepository == null)
                {
                    this.auditLogRepository = new GenericRepository<AuditLog>(context);
                }
                return auditLogRepository;
            }
            set
            {

                this.auditLogRepository = value;

            }
        }

        /*
        public GenericRepository<Course> CourseRepository
        {
            get
            {

                if (this.courseRepository == null)
                {
                    this.courseRepository = new GenericRepository<Course>(context);
                }
                return courseRepository;
            }
        }
        */

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}