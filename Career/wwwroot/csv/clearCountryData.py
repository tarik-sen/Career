import pandas as pd

df = pd.read_csv("worldcities.csv")

df_unique = df.drop_duplicates(subset=["country"])

unique_countries = df_unique["country"].unique()

unique_countries_df = pd.DataFrame({"country": unique_countries})

unique_countries_df.to_csv("countries.csv", index=False)