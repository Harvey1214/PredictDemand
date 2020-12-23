# PredictDemand
 Simple console application. Predicts demand for a product based on past sales.
 This application takes in a dataset consisting of sales figures. It could be a text file or a spreadsheet. When you start the application, write the filename and add parameters (choose a function):
 
    -average    calculates average sales
    
    -median     returns the median sales figure
    
    -trends     calculates the average change in percentages and applies it to the last value, you can set a threshold for this function
    
    -changes    calculates the average absolute changes and applies it to the last value, you can set a threshold for this function
    
    -regression     calculates the future demand using simple linear regression - recommended for most use cases
    
    -auto   runs all of the functions above, and chooses the most accurate one based on the past data

 On certain parameters, you can follow up by setting a threshold value with this syntax: -threshold 1.2
