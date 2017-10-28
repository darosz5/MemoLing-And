using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System;

public enum ACHIEVEMENT_TYPE
{
    Done6,
    Category1starMin,
    Category2starMin,
    Category3starMin,
    All6,
    All1star,
    All2star,
    All3star,  
    None,  
}

public class GPSManager
{

    public static void Activate()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
               // Debug.Log("GPS autenticated " + success);
                GameControl.control.isLoggedIn = success;
            });
        }

        //Debug.Log(Social.localUser.authenticated);

    }

    public static bool IsAutenticated()
    {
        
        return Social.localUser.authenticated;
    }

    public static void OperLeaderBoardsUI()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        Social.ShowLeaderboardUI();
    }

    public static void OpenAchievmentsUI()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        Social.ShowAchievementsUI();
    }

    private static string GetGPSIDLB(string categoryName)
    {
        string boardName = "";
        switch (categoryName)
        {
            case Statics.animals:
                boardName = GPGSIds.leaderboard_animals;
                break;
            case Statics.body:
                boardName = GPGSIds.leaderboard_human_body;
                break;
            case Statics.city:
                boardName = GPGSIds.leaderboard_city;
                break;
            case Statics.clothes:
                boardName = GPGSIds.leaderboard_clothes;
                break;
            case Statics.countries:
                boardName = GPGSIds.leaderboard_countries;
                break;
            case Statics.ecology:
                boardName = GPGSIds.leaderboard_ecology;
                break;
            case Statics.food:
                boardName = GPGSIds.leaderboard_food;
                break;
            case Statics.house:
                boardName = GPGSIds.leaderboard_house;
                break;
            case Statics.human:
                boardName = GPGSIds.leaderboard_everyday_life;
                break;
            case Statics.jobs:
                boardName = GPGSIds.leaderboard_jobs;
                break;
            case Statics.school:
                boardName = GPGSIds.leaderboard_school;
                break;
            case Statics.sport:
                boardName = GPGSIds.leaderboard_sport;
                break;
            case Statics.travel:
                boardName = GPGSIds.leaderboard_travel;
                break;

            default:
                break;
        }
        return boardName;
    }

    public static void SetRecoredForLeaderboard(CategoryState state)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        int totalScore = 0;

        int[] scores = state.scores;

        for (int i = 0; i < scores.Length; i++)
        {
            totalScore += scores[i];
        }

        string boardName = GetGPSIDLB(state.categoryName);

        Social.ReportScore(totalScore, boardName, (bool success) =>
        {
            if (success)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log("fail");
            }
        }
        );
    }

    internal static void CheckForAchievments(CategoryState state)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        bool min6Achievment = CheckFor6minAchievamnt(state);
        if (min6Achievment)
        {
            ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.Done6;
            ReportAchievment(achievmentType, state.categoryName);

            CheckFor6minAchievamntAll();
        }
        CheckFor6minAchievamnt(state);

        bool star1Achieved =  CheckForXStarAchievment(state, 1);      
        if (star1Achieved)
        {
            ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.Category1starMin;
            ReportAchievment(achievmentType, state.categoryName);

            CheckForXStarAchievmentAll(1);
        }

        bool star2Achieved = CheckForXStarAchievment(state, 2);
        if (star2Achieved)
        {
            ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.Category2starMin;
            ReportAchievment(achievmentType, state.categoryName);
            
            CheckForXStarAchievmentAll(2);
        }

        bool star3Achieved = CheckForXStarAchievment(state, 3);
        if (star3Achieved)
        {
            ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.Category3starMin;
            ReportAchievment(achievmentType, state.categoryName);
           
            CheckForXStarAchievmentAll(3);
        }
    }

    private static bool CheckFor6minAchievamnt(CategoryState state)
    {
        
        for (int i = 0; i < 6; i++)
        {
            if (state.levelsState[i] < 2)
                return false;
        }
        
        return true;

    }

    private static void CheckFor6minAchievamntAll()
    {
        ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.All6;
        for (int i = 0; i < GameControl.control.CategoryStates.Count; i++)
        {
            if (!CheckFor6minAchievamnt(GameControl.control.CategoryStates[i]))
                return;
        }
        ReportAchievment(achievmentType);
    }

    private static bool CheckForXStarAchievment(CategoryState state, int numberOfStars)
    {
        
        for (int i = 0; i < state.scores.Length; i++)
        {
            if(GameControl.control.GetStars(state.scores[i], i) < numberOfStars)
            {
                return false;
            }
        }
        
        return true;
        
    }

    private static void CheckForXStarAchievmentAll(int numOfStars)
    {
        ACHIEVEMENT_TYPE achievmentType = GetAchievmentType(numOfStars, true);
        for (int i = 0; i < GameControl.control.CategoryStates.Count; i++)
        {
            if (!CheckForXStarAchievment(GameControl.control.CategoryStates[i], 1))
                return;
        }
        ReportAchievment(achievmentType);
    }

   


    private static ACHIEVEMENT_TYPE GetAchievmentType(int numOfStars, bool all)
    {
        ACHIEVEMENT_TYPE achievmentType = ACHIEVEMENT_TYPE.None;
        switch (numOfStars)
        {
            case 1:
                if (!all)
                {
                    achievmentType = ACHIEVEMENT_TYPE.Category1starMin;
                }
                else
                {
                    achievmentType = ACHIEVEMENT_TYPE.All1star;
                }
                
                break;
            case 2:
                if (!all)
                {
                    achievmentType = ACHIEVEMENT_TYPE.Category2starMin;
                }
                else
                {
                    achievmentType = ACHIEVEMENT_TYPE.All2star;
                }

                break;
            case 3:
                if (!all)
                {
                    achievmentType = ACHIEVEMENT_TYPE.Category3starMin;
                }
                else
                {
                    achievmentType = ACHIEVEMENT_TYPE.All3star;
                }
                break;
            default:
                break;
        }
        return achievmentType;
    }


    private static void ReportAchievment(ACHIEVEMENT_TYPE achievmentType, string categoryName = "")
    {
        string achievmentName = "";
        switch (achievmentType)
        {
            case ACHIEVEMENT_TYPE.All6:
                achievmentName = GPGSIds.achievement_all_normal_levels_in_every_category;       
                break;
            case ACHIEVEMENT_TYPE.All1star:
                achievmentName = GPGSIds.achievement_all_levels_in_every_category_with_1_star_minimum;
                break;
            case ACHIEVEMENT_TYPE.All2star:
                achievmentName = GPGSIds.achievement_all_levels_in_every_category_with_2_stars_minimum;
                break;
            case ACHIEVEMENT_TYPE.All3star:
                achievmentName = GPGSIds.achievement_all_levels_in_every_category_with_3_stars_the_ultimate_achievement;
                break;
            case ACHIEVEMENT_TYPE.Done6:
                switch (categoryName)
                {
                    case Statics.animals:
                        achievmentName = GPGSIds.achievement_animals_all_normal_levels;                      
                        break;
                    case Statics.body:
                        achievmentName = GPGSIds.achievement_human_body_all_normal_levels;
                        break;
                    case Statics.city:
                        achievmentName = GPGSIds.achievement_city_all_normal_levels;
                        break;
                    case Statics.clothes:
                        achievmentName = GPGSIds.achievement_clothes_all_normal_levels;
                        break;
                    case Statics.countries:
                        achievmentName = GPGSIds.achievement_countries_all_normal_levels;
                        break;
                    case Statics.ecology:
                        achievmentName = GPGSIds.achievement_ecology_all_normal_levels;
                        break;
                    case Statics.food:
                        achievmentName = GPGSIds.achievement_food_all_normal_levels;
                        break;
                    case Statics.house:
                        achievmentName = GPGSIds.achievement_house_all_normal_levels;
                        break;
                    case Statics.human:
                        achievmentName = GPGSIds.achievement_everyday_life_all_normal_levels;
                        break;
                    case Statics.jobs:
                        achievmentName = GPGSIds.achievement_jobs_all_normal_levels;
                        break;
                    case Statics.school:
                        achievmentName = GPGSIds.achievement_school_all_normal_levels;
                        break;
                    case Statics.sport:
                        achievmentName = GPGSIds.achievement_sport_all_normal_levels;
                        break;
                    case Statics.travel:
                        achievmentName = GPGSIds.achievement_travel_all_normal_levels;
                        break;
                    default:
                        break;
                }
                break;
            case ACHIEVEMENT_TYPE.Category1starMin:
                switch (categoryName)
                {
                    case Statics.animals:
                        achievmentName = GPGSIds.achievement_animals_all_levels_with_1_star_minimum;
                        break;
                    case Statics.body:
                        achievmentName = GPGSIds.achievement_human_body_all_levels_with_1_star_minimum;
                        break;
                    case Statics.city:
                        achievmentName = GPGSIds.achievement_city_all_levels_with_1_star_minimum;
                        break;
                    case Statics.clothes:
                        achievmentName = GPGSIds.achievement_clothes_all_levels_with_1_star_minimum;
                        break;
                    case Statics.countries:
                        achievmentName = GPGSIds.achievement_countries_all_levels_with_1_star_minimum;
                        break;
                    case Statics.ecology:
                        achievmentName = GPGSIds.achievement_ecology_all_levels_with_1_star_minimum;
                        break;
                    case Statics.food:
                        achievmentName = GPGSIds.achievement_food_all_levels_with_1_star_minimum;
                        break;
                    case Statics.house:
                        achievmentName = GPGSIds.achievement_house_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.human:
                        achievmentName = GPGSIds.achievement_everyday_life_all_levels_with_1_star_minimum;
                        break;
                    case Statics.jobs:
                        achievmentName = GPGSIds.achievement_jobs_all_levels_with_1_star_minimum;
                        break;
                    case Statics.school:
                        achievmentName = GPGSIds.achievement_school_all_levels_with_1_star_minimum;
                        break;
                    case Statics.sport:
                        achievmentName = GPGSIds.achievement_sport_all_levels_with_1_star_minimum;
                        break;
                    case Statics.travel:
                        achievmentName = GPGSIds.achievement_travel_all_levels_with_1_star_minimum;
                        break;
                    default:
                        break;
                }
                break;
            case ACHIEVEMENT_TYPE.Category2starMin:
                switch (categoryName)
                {
                    case Statics.animals:
                        achievmentName = GPGSIds.achievement_animals_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.body:
                        achievmentName = GPGSIds.achievement_human_body_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.city:
                        achievmentName = GPGSIds.achievement_city_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.clothes:
                        achievmentName = GPGSIds.achievement_clothes_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.countries:
                        achievmentName = GPGSIds.achievement_countries_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.ecology:
                        achievmentName = GPGSIds.achievement_ecology_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.food:
                        achievmentName = GPGSIds.achievement_food_all_levels_with_1_star_minimum;
                        break;
                    case Statics.house:
                        achievmentName = GPGSIds.achievement_house_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.human:
                        achievmentName = GPGSIds.achievement_everyday_life_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.jobs:
                        achievmentName = GPGSIds.achievement_jobs_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.school:
                        achievmentName = GPGSIds.achievement_school_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.sport:
                        achievmentName = GPGSIds.achievement_sport_all_levels_with_2_stars_minimum;
                        break;
                    case Statics.travel:
                        achievmentName = GPGSIds.achievement_travel_all_levels_with_2_stars_minimum;
                        break;
                    default:
                        break;
                }
                break;
            case ACHIEVEMENT_TYPE.Category3starMin:
                switch (categoryName)
                {
                    case Statics.animals:
                        achievmentName = GPGSIds.achievement_animals_all_levels_with_3_stars;
                        break;
                    case Statics.body:
                        achievmentName = GPGSIds.achievement_human_body_all_levels_with_3_stars;
                        break;
                    case Statics.city:
                        achievmentName = GPGSIds.achievement_city_all_levels_with_3_stars;
                        break;
                    case Statics.clothes:
                        achievmentName = GPGSIds.achievement_clothes_all_levels_with_3_stars;
                        break;
                    case Statics.countries:
                        achievmentName = GPGSIds.achievement_countries_all_levels_with_3_stars;
                        break;
                    case Statics.ecology:
                        achievmentName = GPGSIds.achievement_ecology_all_levels_with_3_stars;
                        break;
                    case Statics.food:
                        achievmentName = GPGSIds.achievement_food_all_levels_with_3_stars;
                        break;
                    case Statics.house:
                        achievmentName = GPGSIds.achievement_house_all_levels_with_3_stars;
                        break;
                    case Statics.human:
                        achievmentName = GPGSIds.achievement_everyday_life_all_levels_with_3_stars;
                        break;
                    case Statics.jobs:
                        achievmentName = GPGSIds.achievement_jobs_all_levels_with_3_stars;
                        break;
                    case Statics.school:
                        achievmentName = GPGSIds.achievement_school_all_levels_with_3_stars;
                        break;
                    case Statics.sport:
                        achievmentName = GPGSIds.achievement_sport_all_levels_with_3_stars;
                        break;
                    case Statics.travel:
                        achievmentName = GPGSIds.achievement_travel_all_levels_with_3_stars;
                        break;
                    default:
                        break;
                }
                break;           
            case ACHIEVEMENT_TYPE.None:
                break;
            default:
                break;
        }
        Social.ReportProgress(achievmentName, 100d, (bool success) => 
        {
            //Debug.Log("Success: " + success);
        });
        //Debug.Log(achievmentName);
    }
}
