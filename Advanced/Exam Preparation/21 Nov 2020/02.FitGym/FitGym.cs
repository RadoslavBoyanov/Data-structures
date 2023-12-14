using System.Linq;

namespace _02.FitGym
{
    using System;
    using System.Collections.Generic;

    public class FitGym : IGym
    {
        private Dictionary<int, Member> membersById;
        private Dictionary<int, Trainer> trainersById;


        public FitGym()
        {
            this.membersById = new Dictionary<int, Member>();
            this.trainersById = new Dictionary<int, Trainer>();
        }

        public int MemberCount => this.membersById.Count;
        public int TrainerCount => this.trainersById.Count;

        public void AddMember(Member member)
        {
            if (this.membersById.ContainsKey(member.Id))
            {
                throw new ArgumentException();
            }

            this.membersById.Add(member.Id, member);
        }

        public void HireTrainer(Trainer trainer)
        {
            if (this.trainersById.ContainsKey(trainer.Id))
            {
                throw new ArgumentException();
            }

            this.trainersById.Add(trainer.Id, trainer);
        }

        public void Add(Trainer trainer, Member member)
        {
            if (!this.Contains(trainer) || member.Trainer != null)
            {
                throw new ArgumentException();
            }

            if (!this.Contains(member))
            {
                this.membersById[member.Id] = member;
            }

            member.Trainer = trainer;
            trainer.Members.Add(member);

        }

        public bool Contains(Member member) => this.membersById.ContainsKey(member.Id);

        public bool Contains(Trainer trainer) => this.trainersById.ContainsKey(trainer.Id);

        public Trainer FireTrainer(int id)
        {
            if (!this.trainersById.ContainsKey(id))
            {
                throw new ArgumentException();
            }
            
            var trainer = this.trainersById[id];
            this.trainersById.Remove(trainer.Id);

            foreach (var member in trainer.Members)
            {
                member.Trainer = null;
            }

            return trainer;
        }

        public Member RemoveMember(int id)
        {
            if (!this.membersById.ContainsKey(id))
            {
                throw new ArgumentException();
            }

            var member = this.membersById[id];
            this.membersById.Remove(id);

            if (member.Trainer != null)
            {
                member.Trainer.Members.Remove(member);
            }

            return member;
        }

        public IEnumerable<Member>
            GetMembersInOrderOfRegistrationAscendingThenByNamesDescending()
            => this.membersById.Values
                .OrderBy(m => m.RegistrationDate)
                .ThenByDescending(m => m.Name);

        public IEnumerable<Trainer> GetTrainersInOrdersOfPopularity()
            => this.trainersById.Values
                .OrderBy(t => t.Popularity);

        public IEnumerable<Member>
            GetTrainerMembersSortedByRegistrationDateThenByNames(Trainer trainer)
            => trainer.Members
                .OrderBy(m => m.RegistrationDate)
                .ThenBy(m => m.Name);

        public IEnumerable<Member>
            GetMembersByTrainerPopularityInRangeSortedByVisitsThenByNames(int lo, int hi)
            => this.membersById.Values
                .Where(m => m.Trainer.Popularity >= lo && m.Trainer.Popularity <= hi)
                .OrderBy(m => m.Visits)
                .ThenBy(m => m.Name);

        public Dictionary<Trainer, HashSet<Member>>
            GetTrainersAndMemberOrderedByMembersCountThenByPopularity()
            => trainersById.Values
                .OrderBy(t => t.Members.Count)
                .ThenBy(t => t.Popularity)
                .ToDictionary(t => t, t => t.Members);
    }
}