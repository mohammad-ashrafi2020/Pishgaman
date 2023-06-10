using Infrastructure.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Configs;

class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.PhoneNumber).IsUnique();
        builder.HasIndex(b => b.Email).IsUnique();
        builder.HasIndex(b => b.NatinalCode).IsUnique();

        builder.Property(b => b.Email)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(b => b.PhoneNumber)
            .HasMaxLength(12)
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.Family)
            .HasMaxLength(50)
            .IsRequired();


        builder.Property(b => b.FatherName)
            .HasMaxLength(50)
            .IsRequired();
    }

}