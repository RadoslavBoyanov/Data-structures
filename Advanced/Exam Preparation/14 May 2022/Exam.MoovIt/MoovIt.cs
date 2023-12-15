using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private Dictionary<string, Route> routesById;
        private Dictionary<string, Route> routesByDistanceAndPlaces;

        public MoovIt()
        {
            this.routesById = new Dictionary<string, Route>();
            this.routesByDistanceAndPlaces = new Dictionary<string, Route>();
        }

        
        public void AddRoute(Route route)
        {
            if (this.routesById.ContainsKey(route.Id))
            {
                throw new ArgumentException();
            }

            this.routesById.Add(route.Id, route);
            this.routesByDistanceAndPlaces.Add(this.FindKeyForUniqueRoutes(route), route);
        }

        public void RemoveRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            var route = this.routesById[routeId];
            this.routesById.Remove(routeId);
            this.routesByDistanceAndPlaces.Remove(this.FindKeyForUniqueRoutes(route));
        }

        public bool Contains(Route route)
        {
            return this.routesById.ContainsKey(route.Id) ||
                   this.routesByDistanceAndPlaces.ContainsKey(this.FindKeyForUniqueRoutes(route));
        }

        public int Count => this.routesById.Count;

        public Route GetRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            return this.routesById[routeId];
        }

        public void ChooseRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            this.routesById[routeId].Popularity++;
        }
        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {
            var result = this.routesById.Values
                .Where(r =>
                {
                    var startIndex = r.LocationPoints.IndexOf(startPoint);
                    var endIndex = r.LocationPoints.IndexOf(endPoint);

                    return startIndex >= 0 && endIndex > startIndex;
                })
                .OrderByDescending(r => r.IsFavorite)
                .ThenBy(r =>
                {
                    var startIndex = r.LocationPoints.IndexOf(startPoint);
                    var endIndex = r.LocationPoints.IndexOf(endPoint);
                    return endIndex - startIndex;
                })
                .ThenByDescending(r => r.Popularity);

            return result;
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
            => this.routesById.Values
                .Where(r => r.IsFavorite && (r.LocationPoints.Contains(destinationPoint) &&
                                             r.LocationPoints[0] != destinationPoint))
                .OrderBy(r => r.Distance)
                .ThenByDescending(r => r.Popularity);

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
            => this.routesById.Values
                .OrderByDescending(r => r.Popularity)
                .ThenBy(r => r.Distance)
                .ThenBy(r => r.LocationPoints.Count)
                .Take(5);

        public string FindKeyForUniqueRoutes(Route route)
        {
            return route.Distance.ToString() + route.LocationPoints[0]
                                             + route.LocationPoints[route.LocationPoints.Count - 1];
        }
    }
}
