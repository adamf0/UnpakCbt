using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public sealed partial class TemplateJawaban : Entity
    {
        private TemplateJawaban()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public int IdTemplateSoal { get; private set; }
        public string? JawabanText { get; private set; } = null;
        public string? JawabanImg { get; private set; } = null; //file path
        
        public static TemplateJawabanBuilder Update(TemplateJawaban prev) => new TemplateJawabanBuilder(prev);

        public static Result<TemplateJawaban> Create(
        int IdTemplateSoal,
        string? JawabanText = null,
        string? JwabanImg = null
        )
        {
            var asset = new TemplateJawaban
            {
                Uuid = Guid.NewGuid(),
                IdTemplateSoal = IdTemplateSoal,
                JawabanText = JawabanText,
                JawabanImg = JwabanImg
            };

            asset.Raise(new TemplateJawabanCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
