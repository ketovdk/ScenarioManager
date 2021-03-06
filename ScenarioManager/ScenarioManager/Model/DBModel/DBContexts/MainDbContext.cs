﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel.DBContexts
{
    public class MainDbContext:DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<SmartController> Controllers { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SmartThing> SmartThings { get; set; }

        //эти два сета лучше вынести в отдельную базу

        public DbSet<UserLoginInfo> UserLoginInfos { get; set; }
        public DbSet<TokenGuid> TokenGuids { get; set; }
        public DbSet<ControllerScenarios> ControllerScnarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ControllerScenarios>().HasKey(s => new {s.ControllerId, s.ScenarioId});
            modelBuilder.Entity<ControllerScenarios>()                
                .HasOne(p => p.Scenario)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade)
                ;
            modelBuilder.Entity<SmartThing>()
                .HasOne(x => x.UserGroup).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Sensor>()
                .HasOne(x => x.UserGroup).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<UserGroup>()
                .HasOne(p => p.ParentGroup)
                .WithMany(t => t.ChildrenGroups)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sensor>()
                 .HasOne(p => p.Controller)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmartThing>()
                 .HasOne(p => p.Controller)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmartController>()
                 .HasOne(p => p.UserGroup)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                 .HasOne(p => p.UserGroup)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Scenario>()
                .HasOne(p => p.UserGroup)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        MainDbContext IDesignTimeDbContextFactory<MainDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseNpgsql<MainDbContext>("Host=localhost;Port=5432;Database=ScenarioMain;Username=postgres;Password=Qwerty1;Pooling=true");

            return new MainDbContext(optionsBuilder.Options);
        }
    }
}
