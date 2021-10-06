using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiagramGenerator.DataAccess
{
    public class DiagramGeneratorContext : IdentityDbContext<User>
    {
        public DiagramGeneratorContext(DbContextOptions<DiagramGeneratorContext> options) : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }

        public DbSet<Criterion> Criterion { get; set; }

        public DbSet<Diagram> Diagram { get; set; }

        public DbSet<Input> Input { get; set; }

        public DbSet<Method> Method { get; set; }

        public DbSet<Operation> Operation { get; set; }

        public DbSet<Output> Output { get; set; }

        public DbSet<Process> Process { get; set; }

        public DbSet<Requirement> Requirement { get; set; }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<User> User { get; set; }

        //Diagram 

        public DbSet<DiagramClient> DiagramClient { get; set; }

        public DbSet<DiagramRequirement> DiagramRequirement { get; set; }

        public DbSet<DiagramOutput> DiagramOutput { get; set; }

        public DbSet<DiagramSupplier> DiagramSupplier { get; set; }

        public DbSet<DiagramMethod> DiagramMethod { get; set; }

        public DbSet<DiagramInput> DiagramInput { get; set; }

        public DbSet<DiagramCriterion> DiagramCriterion { get; set; }

        public DbSet<UserDiagram> UserDiagram { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Criterion>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Diagram>();
            modelBuilder.Entity<Input>();
            modelBuilder.Entity<Method>();
            modelBuilder.Entity<Output>();
            modelBuilder.Entity<Requirement>();
            modelBuilder.Entity<Supplier>();

            //Diagram

            modelBuilder.Entity<Diagram>();

            modelBuilder.Entity<Process>()
              .HasOne(x => x.Diagram)
              .WithOne(x => x.Process)
              .HasForeignKey<Process>(x => x.DiagramId);

            modelBuilder.Entity<Operation>()
                .HasOne(x => x.Process)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.ProcessId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Process_Operation");

            modelBuilder.Entity<DiagramCriterion>()
                .HasKey(x => new { x.DiagramId, x.CriterionId });
            modelBuilder.Entity<DiagramCriterion>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Criteria)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramCriterion>()
            //    .HasOne(x => x.Criterion)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Criterion);

            modelBuilder.Entity<DiagramInput>()
                .HasKey(x => new { x.DiagramId, x.InputId });
            modelBuilder.Entity<DiagramInput>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Inputs)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramInput>()
            //    .HasOne(x => x.Input)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Input);

            modelBuilder.Entity<DiagramClient>()
                .HasKey(x => new { x.DiagramId, x.ClientId });
            modelBuilder.Entity<DiagramClient>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramClient>()
            //    .HasOne(x => x.Client)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Client);

            modelBuilder.Entity<DiagramMethod>()
                .HasKey(x => new { x.DiagramId, x.MethodId });
            modelBuilder.Entity<DiagramMethod>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Methods)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramMethod>()
            //    .HasOne(x => x.Method)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Method);

            modelBuilder.Entity<DiagramOutput>()
                .HasKey(x => new { x.DiagramId, x.OutputId });
            modelBuilder.Entity<DiagramOutput>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Outputs)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramOutput>()
            //    .HasOne(x => x.Output)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Output);

            modelBuilder.Entity<DiagramRequirement>()
              .HasKey(x => new { x.DiagramId, x.RequirementId });
            modelBuilder.Entity<DiagramRequirement>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Requirements)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<DiagramRequirement>()
            //    .HasOne(x => x.Requirement)
            //    .WithMany(x => x.Diagrams)
            //    .HasForeignKey(x => x.Requirement);

            modelBuilder.Entity<DiagramSupplier>()
                .HasKey(x => new { x.SupplierId, x.DiagramId });
            modelBuilder.Entity<DiagramSupplier>()
                .HasOne(x => x.Diagram)
                .WithMany(x => x.Suppliers)
                .HasForeignKey(x => x.DiagramId);
            //modelBuilder.Entity<InputSupplier>()
            //    .HasOne(x => x.Supplier)
            //    .WithMany(x => x.Inputs)
            //    .HasForeignKey(x => x.SupplierId);

            modelBuilder.Entity<UserDiagram>()
                .HasKey(x => new { x.UserMail, x.DiagramId });
            modelBuilder.Entity<UserDiagram>()
                .HasOne(x => x.User)
                .WithMany(x => x.Diagrams)
                .HasForeignKey(x => x.UserMail);

            base.OnModelCreating(modelBuilder);
        }
    }
}