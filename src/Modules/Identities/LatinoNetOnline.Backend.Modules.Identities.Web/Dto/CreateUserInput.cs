namespace LatinoNetOnline.Backend.Modules.Identities.Web.Dto
{
    record CreateUserInput(string Name, string Lastname, string Username, string Email, string Password, string TermsAndConditionVersion);
}
