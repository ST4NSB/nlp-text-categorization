﻿using System.Collections.Generic;

namespace CategoryConverter
{
    public class Converter
    {
        private static Dictionary<string, string> cat = new Dictionary<string, string>()
        {
            { "1POL", "CURRENT NEWS - POLITICS" },
            {"2ECO",    "CURRENT NEWS - ECONOMICS"},
            {"3SPO",    "CURRENT NEWS - SPORT"},
            {"4GEN",    "CURRENT NEWS - GENERAL"},
            {"6INS",    "CURRENT NEWS - INSURANCE"},
            {"7RSK",    "CURRENT NEWS - RISK NEWS"},
            {"8YDB",    "TEMPORARY"},
            {"9BNX",    "TEMPORARY"},
            {"ADS10", "  CURRENT NEWS - ADVERTISING            "},
            {"BNW14", "  CURRENT NEWS - BUSINESS NEWS          "},
            {"BRP11", "  CURRENT NEWS - BRANDS                 "},
            {"C11",   " STRATEGY/PLANS                         "},
            {"C12",   " LEGAL/JUDICIAL                         "},
            {"C13",   " REGULATION/POLICY                      "},
            {"C14",   " SHARE LISTINGS                         "},
            {"C15",   " PERFORMANCE                            "},
            {"C151",  "  ACCOUNTS/EARNINGS                     "},
            {"C1511", "  ANNUAL RESULTS                        "},
            {"C152",  "  COMMENT/FORECASTS                     "},
            {"C16",   "     INSOLVENCY/LIQUIDITY               "},
            {"C17",   "     FUNDING/CAPITAL                    "},
            {"C171",  "  SHARE CAPITAL                         "},
            {"C172",  "  BONDS/DEBT ISSUES                     "},
            {"C173",  "  LOANS/CREDITS                         "},
            {"C174",  "  CREDIT RATINGS                        "},
            {"C18",   "     OWNERSHIP CHANGES                  "},
            {"C181",  "  MERGERS/ACQUISITIONS                  "},
            {"C182",  "  ASSET TRANSFERS                       "},
            {"C183",  "  PRIVATISATIONS                        "},
            {"C21",   "     PRODUCTION/SERVICES                "},
            {"C22",   "     NEW PRODUCTS/SERVICES              "},
            {"C23",   "     RESEARCH/DEVELOPMENT               "},
            {"C24",   "     CAPACITY/FACILITIES                "},
            {"C31",   "     MARKETS/MARKETING                  "},
            {"C311",  "  DOMESTIC MARKETS                      "},
            {"C312",  "  EXTERNAL MARKETS                      "},
            {"C313",  "  MARKET SHARE                          "},
            {"C32", "ADVERTISING/PROMOTION                     "},
            {"C33", "CONTRACTS/ORDERS                          "},
            {"C331", "   DEFENCE CONTRACTS                     "},
            {"C34", "MONOPOLIES/COMPETITION                    "},
            {"C41", "MANAGEMENT                                "},
            {"C411","    MANAGEMENT MOVES                      "},
            {"C42", "LABOUR                                    "},
            {"CCAT","    CORPORATE/INDUSTRIAL                  "},
            {"E11", "ECONOMIC PERFORMANCE                      "},
            {"E12", "MONETARY/ECONOMIC                         "},
            {"E121","    MONEY SUPPLY                          "},
            {"E13", "INFLATION/PRICES                          "},
            {"E131","    CONSUMER PRICES                       "},
            {"E132","    WHOLESALE PRICES                      "},
            {"E14", "CONSUMER FINANCE                          "},
            {"E141","    PERSONAL INCOME                       "},
            {"E142","    CONSUMER CREDIT                       "},
            {"E143","    RETAIL SALES                          "},
            {"E21", "GOVERNMENT FINANCE                        "},
            {"E211","    EXPENDITURE/REVENUE                   "},
            {"E212","    GOVERNMENT BORROWING                  "},
            {"E31", "OUTPUT/CAPACITY                           "},
            {"E311","    INDUSTRIAL PRODUCTION                 "},
            {"E312","    CAPACITY UTILIZATION                  "},
            {"E313","    INVENTORIES                           "},
            {"E41", "EMPLOYMENT/LABOUR                         "},
            {"E411","    UNEMPLOYMENT                          "},
            {"E51", "TRADE/RESERVES                            "},
            {"E511","    BALANCE OF PAYMENTS                   "},
            {"E512","    MERCHANDISE TRADE                     "},
            {"E513","    RESERVES                              "},
            {"E61", "HOUSING STARTS                            "},
            {"E71", "LEADING INDICATORS                        "},
            {"ECAT","    ECONOMICS                             "},
            {"ENT12","   CURRENT NEWS - ENTERTAINMENT          "},
            {"G11", "SOCIAL AFFAIRS                            "},
            {"G111","    HEALTH/SAFETY                         "},
            {"G112","    SOCIAL SECURITY                       "},
            {"G113","    EDUCATION/RESEARCH                    "},
            {"G12", "INTERNAL POLITICS                         "},
            {"G13", "INTERNATIONAL RELATIONS                   "},
            {"G131","    DEFENCE                               "},
            {"G14", "ENVIRONMENT                               "},
            {"G15", "EUROPEAN COMMUNITY                        "},
            {"G151","    EC INTERNAL MARKET                    "},
            {"G152","    EC CORPORATE POLICY                   "},
            {"G153","    EC AGRICULTURE POLICY                 "},
            {"G154","    EC MONETARY/ECONOMIC                  "},
            {"G155","    EC INSTITUTIONS                       "},
            {"G156","    EC ENVIRONMENT ISSUES                 "},
            {"G157","    EC COMPETITION/SUBSIDY                "},
            {"G158","    EC EXTERNAL RELATIONS                 "},
            {"G159","    EC GENERAL                            "},
            {"GCAT","    GOVERNMENT/SOCIAL                     "},
            {"GCRIM","   CRIME, LAW ENFORCEMENT                "},
            {"GDEF", "   DEFENCE                               "},
            {"GDIP", "   INTERNATIONAL RELATIONS               "},
            {"GDIS", "   DISASTERS AND ACCIDENTS               "},
            {"GEDU", "   EDUCATION                             "},
            {"GENT", "   ARTS, CULTURE, ENTERTAINMENT          "},
            {"GENV", "   ENVIRONMENT AND NATURAL WORLD         "},
            {"GFAS", "   FASHION                               "},
            {"GHEA", "   HEALTH                                "},
            {"GJOB", "   LABOUR ISSUES                         "},
            {"GMIL", "   MILLENNIUM ISSUES                     "},
            {"GOBIT","   OBITUARIES                            "},
            {"GODD", "   HUMAN INTEREST                        "},
            {"GPOL", "   DOMESTIC POLITICS                     "},
            {"GPRO", "   BIOGRAPHIES, PERSONALITIES, PEOPLE    "},
            {"GREL", "   RELIGION                              "},
            {"GSCI", "   SCIENCE AND TECHNOLOGY                "},
            {"GSPO", "   SPORTS                                "},
            {"GTOUR","   TRAVEL AND TOURISM                    "},
            {"GVIO", "   WAR, CIVIL WAR                        "},
            {"GVOTE","   ELECTIONS                             "},
            {"GWEA", "   WEATHER                               "},
            {"GWELF","   WELFARE, SOCIAL SERVICES              "},
            {"M11", "EQUITY MARKETS                            "},
            {"M12", "BOND MARKETS                              "},
            {"M13", "MONEY MARKETS                             "},
            {"M131","    INTERBANK MARKETS                     "},
            {"M132","    FOREX MARKETS                         "},
            {"M14", "COMMODITY MARKETS                         "},
            {"M141","    SOFT COMMODITIES                      "},
            {"M142","    METALS TRADING                        "},
            {"M143","    ENERGY MARKETS                        "},
            {"MCAT","    MARKETS                               "},
            {"MEUR","    EURO CURRENCY                         "},
            {"PRB13","   CURRENT NEWS - PRESS RELEASE WIRES    "},
        };

        public static string GetCategoryFullName(string category)
        {
            return cat[category.ToUpper()].TrimStart().TrimEnd();
        }
    }
}
