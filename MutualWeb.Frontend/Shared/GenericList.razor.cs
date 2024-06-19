using Microsoft.AspNetCore.Components;

namespace MutualWeb.Frontend.Shared
{
    public partial class GenericList<Titem>
    {
        [Parameter]
        public RenderFragment? Loading { get; set; }

        [Parameter]
        public bool IsLoading { get; set; } = false;

        [Parameter]
        public RenderFragment? NoRecords { get; set; }

        [EditorRequired]
        [Parameter]
        public RenderFragment Body { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public List<Titem> MyList { get; set; } = null!;
    }
}
