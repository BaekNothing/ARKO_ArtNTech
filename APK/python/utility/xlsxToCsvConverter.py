import pandas as pd
import os 

file_list = os.listdir('./')
for f in file_list:
    if f.endswith(".xlsx"):
        print(f)
        df = pd.read_excel(f)
        data_frame = pd.DataFrame(df)
        for colName in data_frame:
            if colName.startswith('//'):
                df.drop(colName, axis=1, inplace=True)
        df.to_csv(f.replace('.xlsx', '.csv'), index=False)
