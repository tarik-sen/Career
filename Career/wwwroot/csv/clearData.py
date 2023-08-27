import pandas as pd

df = pd.read_csv("gics-map-2018.csv")

df_unique = df.drop_duplicates(subset=["Sector", "Industry"])

unique_sectors = df_unique["Sector"].unique()
unique_industries = df_unique["Industry"].unique()

unique_sectors_df = pd.DataFrame({"sector": unique_sectors})
unique_sectors_df.to_csv("sectors.csv", index=False)

unique_industries_df = pd.DataFrame({"industry": unique_industries})
unique_industries_df.to_csv("industries.csv", index=False)