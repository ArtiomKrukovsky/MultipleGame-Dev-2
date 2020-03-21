using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RatingController : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;

    private void Start()
    {
        UpdateRating();
    }

    public void UpdateRating()
    {
        try
        {
            if (content == null || prefab == null)
            {
                Debug.Log($"Error, something went wrong");
                return;
            }

            var ratings = new List<RatingModel>();

            using (SqlConnection dbConnection = new SqlConnection(DbHelper.ConnectionString))
            {
                dbConnection.Open();

                string query = "SELECT SUBSTRING(Team, CHARINDEX('_',Team) + 2, LEN(Team) - CHARINDEX('_', Team)), Map, SUM(Score) " +
                               "FROM Rating " +
                               "GROUP BY SUBSTRING(Team, CHARINDEX('_',Team) + 2, LEN(Team) - CHARINDEX('_', Team)), Map " +
                               "ORDER BY SUM(Score) DESC";
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            dbConnection.Close();
                            return;
                        }

                        while (reader.Read())
                        {
                            ratings.Add(new RatingModel
                            {
                                Team = reader.GetString(0),
                                Map = reader.GetString(1),
                                Score = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            if (ratings.Any())
            {
                ratings = ratings.Select((item, index) => { item.Rating = index + 1; return item; }).ToList();

                foreach (var rating in ratings)
                {
                    SetRatingParametrs(prefab, content, rating);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
            return;
        }
    }

    private void SetRatingParametrs(RectTransform prefab, RectTransform content, RatingModel rating)
    {
        var instance = Instantiate(prefab.gameObject) as GameObject;
        instance?.transform.SetParent(content, false);
        InitializeInstanceView(instance, rating);
    }

    private void InitializeInstanceView(GameObject viewGameObject, RatingModel model)
    {
        if (viewGameObject == null)
        {
            return;
        }

        RatingViewModel view = new RatingViewModel(viewGameObject.transform);
        view.Rating.text = model.Rating.ToString();
        view.Team.text = model.Team;
        view.Map.text = model.Map;
        view.Score.text = model.Score.ToString();
    }

    private class RatingModel
    {
        public int Rating { get; set; }
        public string Team { get; set; }
        public string Map { get; set; }
        public int Score { get; set; }
    }

    private class RatingViewModel
    {
        public Text Rating { get; set; }
        public Text Team { get; set; }
        public Text Map { get; set; }
        public Text Score { get; set; }

        public RatingViewModel(Transform rootView)
        {
            Rating = rootView.Find("Rating").GetComponent<Text>();
            Team = rootView.Find("Team").GetComponent<Text>();
            Map = rootView.Find("Map").GetComponent<Text>();
            Score = rootView.Find("Score").GetComponent<Text>();
        }
    }
}
