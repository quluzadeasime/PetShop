using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstracts
{
    public interface IMemberService
    {
        void Add(Member member);
        void Delete(int id);
        void Update(int id, Member member);
        Member Get(Func<Member, bool> func = null);
        List<Member> GetAll(Func<Member, bool> func = null);
    }
}
