﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseConnection")
        {
        }

        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<SubMenuMaster> SubMenuMasters { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<Usermaster> Usermasters { get; set; }
        public DbSet<SavedMenuRoles> SavedMenuRoles { get; set; }
        public DbSet<SavedSubMenuRoles> SavedSubMenuRoles { get; set; }
        public DbSet<PasswordMaster> PasswordMaster { get; set; }
        public DbSet<SavedAssignedRoles> SavedAssignedRoles { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.BuyerOrderNumber> BuyerOrderNumbers { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.BuyerReference> BuyerReferences { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.Buyername> Buyernames { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.SetNote> SetNotes { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.Unit> Units { get; set; }

        public System.Data.Entity.DbSet<ManageRoles.Repository.Supplier> Suppliers { get; set; }
    }
}
