using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using DiagramGenerator.Web.ViewModels;
using DiagramGenerator.Web.ViewModels.Account;
using DiagramGenerator.Web.ViewModels.Diagram;
using DiagramGenerator.Web.ViewModels.Pages;

namespace DiagramGenerator.Web.Mappings
{
    public class DiagramGeneratorMappingProfile : Profile
    {
        public DiagramGeneratorMappingProfile()
        {
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();

            CreateMap<Input, InputViewModel>().ReverseMap();

            CreateMap<Requirement, RequirementViewModel>().ReverseMap();

            CreateMap<Method, MethodViewModel>().ReverseMap();

            CreateMap<Client, ClientViewModel>().ReverseMap();

            CreateMap<Supplier, SupplierViewModel>().ReverseMap();

            CreateMap<Criterion, CriterionViewModel>().ReverseMap();

            CreateMap<Input, DiagramInputViewModel>().ReverseMap();

            CreateMap<Requirement, DiagramRequirementViewModel>().ReverseMap();

            CreateMap<Method, DiagramMethodViewModel>().ReverseMap();

            CreateMap<Client, DiagramClientViewModel>().ReverseMap();

            CreateMap<Supplier, DiagramSupplierViewModel>().ReverseMap();

            CreateMap<Criterion, DiagramCriterionViewModel>().ReverseMap();

            CreateMap<Output, OutputViewModel>().ReverseMap();

            CreateMap<Output, DiagramOutputViewModel>().ReverseMap();

            CreateMap<Operation, OperationViewModel>()
                 .ForPath(o => o.DiagramId, ex => ex.MapFrom(o => o.Process.DiagramId));

            CreateMap<OperationViewModel, Operation>()
                .ForPath(o => o.Process.DiagramId, ex => ex.MapFrom(o => o.DiagramId));

            CreateMap<Output, OutputViewModel>().ReverseMap();

            CreateMap<Process, ProcessPageViewModel>().ReverseMap();

            CreateMap<Process, ProcessViewModel>().ReverseMap();

            CreateMap<Diagram, DiagramViewModel>().ReverseMap();

            CreateMap<User, SignUpViewModel>();

            CreateMap<SignUpViewModel, User>()
                .ForMember(o => o.Email, ex => ex.MapFrom(o => o.Username));

            // DiagramInput
            CreateMap<DiagramInput, DiagramInputViewModel>()
                .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
                .ForMember(o => o.Id, ex => ex.MapFrom(o => o.InputId))
                .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Input.Description))
                .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Input.Name))
                .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Input.Lp))
                .ForMember(o => o.Quantity, ex => ex.MapFrom(o => o.Quantity));

            CreateMap<DiagramInputViewModel, DiagramInput>()
                .ForPath(o => o.Input.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Input.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Input.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Input.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Input.Type, ex => ex.MapFrom(o => o.Type))
                .ForPath(o => o.Input.UserEmail, ex => ex.MapFrom(o => o.UserEmail))  
                .ForMember(o => o.InputId, ex => ex.MapFrom(o => o.Id));

            // DiagramOutput
            CreateMap<DiagramOutput, DiagramOutputViewModel>()
              .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
              .ForMember(o => o.Id, ex => ex.MapFrom(o => o.OutputId))
              .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Output.Description))
              .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Output.Name))
              .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Output.Lp))
              .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Output.UserEmail))
              .ForMember(o => o.Quantity, ex => ex.MapFrom(o => o.Quantity));

            CreateMap<DiagramOutputViewModel, DiagramOutput>()
                .ForPath(o => o.Output.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Output.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Output.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Output.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Output.Type, ex => ex.MapFrom(o => o.Type))
                .ForPath(o => o.Output.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.OutputId, ex => ex.MapFrom(o => o.Id));

            // DiagramCriterion 
            CreateMap<DiagramCriterion, DiagramCriterionViewModel>()
             .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
             .ForMember(o => o.Id, ex => ex.MapFrom(o => o.CriterionId))
             .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Criterion.Description))
             .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Criterion.Name))
             .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Criterion.Lp))
             .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Criterion.UserEmail));

            CreateMap<DiagramCriterionViewModel, DiagramCriterion>()
                .ForPath(o => o.Criterion.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Criterion.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Criterion.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Criterion.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Criterion.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.CriterionId, ex => ex.MapFrom(o => o.Id));

            // DiagramMethod
            CreateMap<DiagramMethodViewModel, DiagramMethod>()
                .ForPath(o => o.Method.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Method.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Method.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Method.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Method.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.MethodId, ex => ex.MapFrom(o => o.Id));

            CreateMap<DiagramMethod, DiagramMethodViewModel>()
                 .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
                 .ForMember(o => o.Id, ex => ex.MapFrom(o => o.MethodId))
                 .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Method.Description))
                 .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Method.Name))
                 .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Method.Lp))
                 .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Method.UserEmail));

            // DiagramRequirement
            CreateMap<DiagramRequirementViewModel, DiagramRequirement>()
                .ForPath(o => o.Requirement.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Requirement.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Requirement.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Requirement.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Requirement.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.RequirementId, ex => ex.MapFrom(o => o.Id));

            CreateMap<DiagramRequirement, DiagramRequirementViewModel>()
                 .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
                 .ForMember(o => o.Id, ex => ex.MapFrom(o => o.RequirementId))
                 .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Requirement.Description))
                 .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Requirement.Name))
                 .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Requirement.Lp))
                 .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Requirement.UserEmail));

            // DiagramClient
            CreateMap<DiagramClientViewModel, DiagramClient>()
                .ForPath(o => o.Client.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Client.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Client.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Client.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Client.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.ClientId, ex => ex.MapFrom(o => o.Id));

            CreateMap<DiagramClient, DiagramClientViewModel>()
                .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
                .ForMember(o => o.Id, ex => ex.MapFrom(o => o.ClientId))
                .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Client.Description))
                .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Client.Name))
                .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Client.Lp))
                .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Client.UserEmail));

            // Diagram Supplier
            CreateMap<DiagramSupplierViewModel, DiagramSupplier>()
                .ForPath(o => o.Supplier.Name, ex => ex.MapFrom(o => o.Name))
                .ForPath(o => o.Supplier.Description, ex => ex.MapFrom(o => o.Description))
                .ForPath(o => o.Supplier.Id, ex => ex.MapFrom(o => o.Id))
                .ForPath(o => o.Supplier.Lp, ex => ex.MapFrom(o => o.Lp))
                .ForPath(o => o.Supplier.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                .ForMember(o => o.SupplierId, ex => ex.MapFrom(o => o.Id));

            CreateMap<DiagramSupplier, DiagramSupplierViewModel>()
                .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.DiagramId))
                .ForMember(o => o.Id, ex => ex.MapFrom(o => o.SupplierId))
                .ForMember(o => o.Description, ex => ex.MapFrom(o => o.Supplier.Description))
                .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Supplier.Name))
                .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Supplier.Lp))
                .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.Supplier.UserEmail));

            CreateMap<Diagram, DiagramSummaryPageViewModel>()
                 .ForMember(o => o.DiagramId, ex => ex.MapFrom(o => o.Id))
                 .ForMember(o => o.UserEmail, ex => ex.MapFrom(o => o.UserEmail))
                 .ForMember(o => o.Lp, ex => ex.MapFrom(o => o.Lp));

            //CreateMap<Supplier, SupplierViewModel>()
            //    .ForMember(o => o.Id, ex => ex.MapFrom(o => o.Id));
        }

    }
}
