namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        private readonly Dictionary<string, Website> websitesByDomains = new Dictionary<string, Website>(); 
        private readonly Dictionary<string, Coupon> couponsByCodes = new Dictionary<string, Coupon>();

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.Exist(website))
            {
                throw new ArgumentException();
            }

            this.websitesByDomains[website.Domain].Coupons.Add(coupon);
            coupon.Website = website;
            this.couponsByCodes.Add(coupon.Code, coupon);
        }

        public bool Exist(Website website)
            => this.websitesByDomains.ContainsKey(website.Domain);

        public bool Exist(Coupon coupon)
            => this.couponsByCodes.ContainsKey(coupon.Code);

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!this.websitesByDomains.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            return this.websitesByDomains[website.Domain].Coupons;
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
            => this.couponsByCodes.Values.OrderByDescending(c => c.Validity)
            .ThenByDescending(c => c.DiscountPercentage);

        public IEnumerable<Website> GetSites()
            => this.websitesByDomains.Values;

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
            => this.websitesByDomains.Values.OrderBy(w => w.UsersCount)
            .ThenByDescending(w => w.Coupons.Count);

        public void RegisterSite(Website website)
        {
            if (this.websitesByDomains.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            this.websitesByDomains.Add(website.Domain, website);
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!this.couponsByCodes.ContainsKey(code))
            {
                throw new ArgumentException();
            }

            var coupon = this.couponsByCodes[code];
            var couponWebsite = coupon.Website;

            this.websitesByDomains[couponWebsite.Domain].Coupons.Remove(coupon);

            this.couponsByCodes.Remove(code);

            return coupon;
        }

        public Website RemoveWebsite(string domain)
        {
            if (!this.websitesByDomains.ContainsKey(domain))
            {
                throw new ArgumentException();
            }

            var website = this.websitesByDomains[domain];
            this.websitesByDomains.Remove(domain);

            foreach (var coupon in website.Coupons)
            {
                this.couponsByCodes.Remove(coupon.Code);
            }

            return website;
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if (!this.Exist(website) || !this.Exist(coupon) || !website.Coupons.Contains(coupon))
            {
                throw new ArgumentException();
            }

            this.websitesByDomains[website.Domain].Coupons.Remove(coupon);
            this.couponsByCodes.Remove(coupon.Code);
        }
    }
}
