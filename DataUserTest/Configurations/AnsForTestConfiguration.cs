using DataUserTest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataUserTest.Configurations
{
    public class AnsForTestConfiguration : IEntityTypeConfiguration<AnsForTest>
    {
        public void Configure(EntityTypeBuilder<AnsForTest> builder)
        {
            builder.ToTable("AnsForTests");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ans).IsRequired(true);

            builder.Property(x => x.status).IsRequired(true);

            builder.HasOne(x => x.TestEng).WithMany(x => x.AnsForTests).HasForeignKey(x => x.IdTest);
        }
    }
}
