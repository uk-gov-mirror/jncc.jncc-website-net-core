using JNCC.PublicWebsite.Core.Extensions;
using JNCC.PublicWebsite.Core.Interfaces.Services;
using JNCC.PublicWebsite.Core.Models;
using JNCC.PublicWebsite.Core.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace JNCC.PublicWebsite.Core.Services
{
    internal sealed class MainNavigationService : IMainNavigationService
    {
        private const int _maximumMenuLevel = 3;
        private readonly string[] DisallowedDocumentTypeAliases;

        public MainNavigationService()
        {
            DisallowedDocumentTypeAliases = new string[]
            {
                ScienceCategoryPage.ModelTypeAlias,
                ScienceDetailsPage.ModelTypeAlias,
                SimpleScienceCategoryPage.ModelTypeAlias,
                SimpleScienceDetailsPage.ModelTypeAlias
            };
        }

        public IEnumerable<MainNavigationItemViewModel> GetRootMenuItems(IPublishedContent root, IPublishedContent currentPage)
        {
            var isCurrentPageRoot = currentPage.IsEqual(root);

            if (isCurrentPageRoot)
            {
                return GetMenuItems(root);
            }

            return GetMenuItems(root, currentPage);
        }

        private IEnumerable<MainNavigationItemViewModel> GetMenuItems(IPublishedContent parent, IPublishedContent currentPage = null)
        {
            var menuItems = new List<MainNavigationItemViewModel>();

            if (parent.AreChildrenVisible() == false)
            {
                return menuItems;
            }

            foreach (var item in parent.VisibleChildren()
                                       .Where(x => DisallowedDocumentTypeAliases.Contains(x.ContentType.Alias) == false))
            {
                var menuItem = ToMenuItem(item, currentPage);

                if (item.Level < _maximumMenuLevel)
                {
                    var childItems = GetMenuItems(item, currentPage);

                    if (childItems != null && childItems.Any())
                    {
                        menuItem.Children = childItems;
                    }
                }

                menuItems.Add(menuItem);
            }

            return menuItems;
        }

        private MainNavigationItemViewModel ToMenuItem(IPublishedContent content, IPublishedContent currentPage = null)
        {
            return new MainNavigationItemViewModel()
            {
                Text = content.Name,
                Url = content.Url(),
                IsActive = currentPage != null && content.IsAncestorOrSelf(currentPage)
            };
        }
    }
}
