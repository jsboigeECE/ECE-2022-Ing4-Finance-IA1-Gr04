#!/usr/bin/env python
# coding: utf-8
import numpy as np 
import pandas as pd
from tensorflow.keras import Model, Sequential
from tensorflow.keras.callbacks import EarlyStopping
from tensorflow.keras.layers import Dense, Dropout, Flatten, Input
from tensorflow.keras.utils import to_categorical


# In[ ]:
#Construction d'un tableau np à partir du sudoku en entrée
tableau_cellules=sudoku.Cellules
#tableau_cellules=((0,0,0,0,9,4,0,3,0),
            #(0,0,0,5,1,0,0,0,7),
            #(0,8,9,0,0,0,0,4,0),
            #(0,0,0,0,0,0,2,0,8),
            #(0,6,0,2,0,1,0,5,0),
            #(1,0,2,0,0,0,0,0,0),
            #(0,7,0,0,0,0,5,2,0),
            #(9,0,0,0,6,5,0,0,0),
            #(0,4,0,9,7,0,0,0,0))
sudoku_np = np.asarray(tableau_cellules)
sudoku_np = np.expand_dims(sudoku_np, axis = 0)


from tensorflow.keras.models import load_model
model = load_model(modelPath)
#batch_smart_solve(grids,solver)

def batch_smart_solve(grids, solver):
    """
    NOTE : This function is ugly, feel free to optimize the code ...
    
    This function solves quizzes in the "smart" way. 
    It will fill blanks one after the other. Each time a digit is filled, 
    the new grid will be fed again to the solver to predict the next digit. 
    Again and again, until there is no more blank
    
    Parameters
    ----------
    grids (np.array), shape (?, 9, 9): Batch of quizzes to solve (smartly ;))
    solver (keras.model): The neural net solver
    
    Returns
    -------
    grids (np.array), shape (?, 9, 9): Smartly solved quizzes.
    """
    grids = grids.copy()
    for _ in range((grids == 0).sum((1, 2)).max()):
        preds = np.array(solver.predict(to_categorical(grids)))  # get predictions
        probs = preds.max(2).T  # get highest probability for each 81 digit to predict
        values = preds.argmax(2).T + 1  # get corresponding values
        zeros = (grids == 0).reshape((grids.shape[0], 81))  # get blank positions

        for grid, prob, value, zero in zip(grids, probs, values, zeros):
            if any(zero):  # don't try to fill already completed grid
                where = np.where(zero)[0]  # focus on blanks only
                confidence_position = where[prob[zero].argmax()]  # best score FOR A ZERO VALUE (confident blank)
                confidence_value = value[confidence_position]  # get corresponding value
                grid.flat[confidence_position] = confidence_value  # fill digit inplace
    return grids


input_shape=(9,9,10)



model = Sequential()
model.add(Dense(64, activation='relu', input_shape=input_shape))
model.add(Dropout(0.4))
model.add(Dense(64, activation='relu'))
model.add(Dropout(0.4))
model.add(Flatten())

grid = Input(shape=input_shape)
features = model(grid)
digit_placeholders = [
    Dense(9, activation='softmax')(features)
    for i in range(81)
]
solver = Model(grid, digit_placeholders)  # build the whole model
solver.compile(
    optimizer='adam',
    loss='categorical_crossentropy',
    metrics=['accuracy']
)

solvedsudoku=batch_smart_solve(sudoku_np, solver)[0, :, :]

#for i in range(9):
                #for j in range(9):
                    #index = i * 9 + j
                    #sudoku.Cellules[i][j] = solvesudoku



