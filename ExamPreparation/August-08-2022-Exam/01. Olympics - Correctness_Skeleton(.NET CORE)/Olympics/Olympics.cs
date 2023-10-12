using System;
using System.Collections.Generic;
using System.Linq;

public class Olympics : IOlympics
{
    private Dictionary<int, Competition> competitionsById = new Dictionary<int, Competition>();

    private Dictionary<int, Competitor> competitorsById = new Dictionary<int, Competitor>();

    public void AddCompetition(int id, string name, int participantsLimit)
    {
        if (this.competitionsById.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        Competition competition = new Competition(name, id, participantsLimit);
        this.competitionsById.Add(id, competition);
    }

    public void AddCompetitor(int id, string name)
    {
        if (this.competitorsById.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        Competitor competitor = new Competitor(id, name);
        this.competitorsById.Add(id, competitor);
    }

    public void Compete(int competitorId, int competitionId)
    {
        if (!this.competitionsById.ContainsKey(competitionId) || !this.competitorsById.ContainsKey(competitorId))
        {
            throw new ArgumentException();
        }

        this.competitionsById[competitionId].Competitors.Add(this.competitorsById[competitorId]);

        this.competitorsById[competitorId].TotalScore += this.competitionsById[competitionId].Score;
    }

    public void Disqualify(int competitionId, int competitorId)
    {
        if (!this.competitionsById.ContainsKey(competitionId) || !this.competitorsById.ContainsKey(competitorId))
        {
            throw new ArgumentException();
        }

        Competition competition = this.competitionsById[competitionId];

        competition.Competitors.Remove(this.competitorsById[competitorId]);
        this.competitorsById[competitorId].TotalScore -= this.competitionsById[competitionId].Score;
    }

    public IEnumerable<Competitor> GetByName(string name)
    {
        var competirors = this.competitorsById.Values.Where(c =>c.Name == name).OrderBy(c=>c.Id).ToList();

        if (competirors.Count == 0)
        {
            throw new ArgumentException();
        }

        return competirors;
    }

    public IEnumerable<Competitor> FindCompetitorsInRange(long min, long max)
        => this.competitorsById.Values.Where(c => c.TotalScore > min && c.TotalScore <= max).OrderBy(c=>c.Id);

    public IEnumerable<Competitor> SearchWithNameLength(int min, int max)
        => this.competitorsById.Values.Where(c=> c.Name.Length >= min && c.Name.Length <= max).OrderBy(c=> c.Id);

    public bool Contains(int competitionId, Competitor comp)
    {
        if (!this.competitionsById.ContainsKey(competitionId))
        {
            throw new ArgumentException();
        }

        var competition = this.competitionsById[competitionId];
        var competitor = this.competitorsById[comp.Id];

        return competition.Competitors.Contains(competitor);
    }

    public int CompetitionsCount()
        => this.competitionsById.Values.Count;

    public int CompetitorsCount()
        => this.competitorsById.Values.Count;


    public Competition GetCompetition(int id)
    {
        if (!this.competitionsById.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        return this.competitionsById[id];
    }
}