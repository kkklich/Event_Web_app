

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace KLich
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class MeetingsEntities : DbContext
{
    public MeetingsEntities()
        : base("name=MeetingsEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Meet> Meets { get; set; }

    public virtual DbSet<Meet_participant> Meet_participant { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

}

}

