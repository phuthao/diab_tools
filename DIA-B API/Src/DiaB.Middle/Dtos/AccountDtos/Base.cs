using CpTech.Core.Middle.Dtos;
using CoreFilterDto = DiaB.Middle.Dtos.BaseDtos.CoreFilterDto;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
        }

        public partial class AppItem : BaseItemDto
        {
        }

        public partial class AppItemPortal : BaseItemDto
        {
        }

        public partial class Filter : CoreFilterDto
        {
        }

        public partial class Item : CoreItemDto
        {
        }
    }
}
