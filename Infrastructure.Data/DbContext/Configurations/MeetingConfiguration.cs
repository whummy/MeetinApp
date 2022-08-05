using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DbContext.Configurations
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {

            builder.Property(x => x.Title).IsRequired();
            builder.HasOne(x => x.Creator)
                 .WithMany()
                 .HasForeignKey(x => x.CreatedById)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
