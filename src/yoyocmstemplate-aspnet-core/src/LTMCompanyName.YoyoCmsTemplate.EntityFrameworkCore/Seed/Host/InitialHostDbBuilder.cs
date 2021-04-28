using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public InitialHostDbBuilder(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
        //    new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            //new DropdownCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
