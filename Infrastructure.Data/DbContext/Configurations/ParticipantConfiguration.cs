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
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {

            //builder.Property(x => x.Title).IsRequired();
            builder.HasKey(x => new { x.MeetingId, x.UserId });
            builder.HasOne(x => x.Meeting)
                 .WithMany(x => x.Participants)
                 .HasForeignKey(x => x.MeetingId)
                 .OnDelete(DeleteBehavior.NoAction);    
                 
        }
    }
}
