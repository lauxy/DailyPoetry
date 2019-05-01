using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyPoetry.Models.KnowledgeModels
{
    // todo: optimize SQL
    public interface IDbItem<ST>
    {
        ST ToSimplified();
    }
}
