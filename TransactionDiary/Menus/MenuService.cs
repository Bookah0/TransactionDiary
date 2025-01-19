
public class MenuService
{
    public Menu currentMenu;
    public Menu? previousMenu;
    public Dictionary<MenuType, Menu> Menus {get;set;} = [];

    public MenuService(TransactionService tService, UserService uService)
    {
        var loginMenu = new LoginMenu(this, tService, uService);
        currentMenu = loginMenu;
        
        Menus.Add(MenuType.login, loginMenu);
        Menus.Add(MenuType.overview, new OverviewMenu(this, tService, uService));
        Menus.Add(MenuType.edit, new EditTransactionMenu(this, tService, uService));
        Menus.Add(MenuType.add, new AddTransactionMenu(this, tService, uService));

        currentMenu.Display();
    }

    public void SetMenu(Menu menu)
    {
        previousMenu = currentMenu;
        currentMenu = menu;
        currentMenu.Display();
    }

    public void SetMenu(MenuType menuType)
    {
        previousMenu = currentMenu;
        currentMenu = Menus[menuType];
        currentMenu.Display();
    }

    public void SetMenuToPrevious()
    {
        if(previousMenu != null)
        {
            SetMenu(previousMenu);
        }
    }
}