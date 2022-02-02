#!/usr/bin/env python
# coding: utf-8

# # Neural nets as Sudoku solvers
# 
# Since the dataset was initially created for neural-nets, I focus on this approach. Obviously, there are other ways to solve Sudoku puzzle, some of them are way more intuitive than neural-nets. However, it was an opportunity to get a first hands-on with Keras framework !
# 
# I read the https://github.com/Kyubyong/sudoku to see if CNNs can crack Sudoku puzzles. 
# I was wondering if we could achieve similar results with a simpler neural net architecture.
# 
# I decided to dig more in the direction of the following tip Kyubyong mentionned about predicting :
# > I use a simple trick in inference. Instead of cracking the whole blanks all at once, I fill in a single blank where the prediction is the most probable among the all predictions.
# 
# I used the same trick but in **both prediction phase AND training phase.**
# One of my workmate came up with this idea. Thanks to him !
# 
# In a nutshell : 
# * **step 1 - Calibration**: train network to reproduce a correct grid identically.
# * **step 2 - Pull off** : for each training quizz, pull off one digit, and train to recover it.
# * **step 3 - pull off one more**: pull off 2 digits (the first one may be different than *step 2*).
# * **...**
# * **penultimate step**: pull off a reasonible number off digits to build a realistic Sudoku grid (arround 50)
# * **final step**: Predict smartly. Take a quizz, get predictions, place the digit which the network is more confident with to complete one single blank. Then feed the new quizz (with one blank less) into the network again. And so on until there is no more blank.
# 
# It works pretty well, with a final **99.7 % accuracy**. Feel free to fine-tune the model and share your results :).
# 
# Merry end of the year !
# 
# Dithyrambe
# 
# *NOTE : Training the network is quite long, you'll have to run the notebook on your environment since Kaggle kills it after 3600 sec*

# In[6]:


# imports
import numpy as np
import pandas as pd

from tensorflow.keras import Model, Sequential
from tensorflow.keras.callbacks import EarlyStopping
from tensorflow.keras.layers import Dense, Dropout, Flatten, Input
from tensorflow.keras.utils import to_categorical


# Define some useful functions. Docstrings should be enough for explanations. The whole trick is in the `batch_smart_solve` function.

# In[7]:


def load_data(nb_train=40000, nb_test=10000, full=False):
    """
    Function to load data in the keras way.
    
    Parameters
    ----------
    nb_train (int): Number of training examples
    nb_test (int): Number of testing examples
    full (bool): If True, whole csv will be loaded, nb_test will be ignored
    
    Returns
    -------
    Xtrain, ytrain (np.array, np.array),
        shapes (nb_train, 9, 9), (nb_train, 9, 9): Training samples
    Xtest, ytest (np.array, np.array), 
        shapes (nb_test, 9, 9), (nb_test, 9, 9): Testing samples
    """
    # if full is true, load the whole dataset
    if full:
        sudokus = pd.read_csv('C:/Users/srabh/Desktop/sudoku.csv').values
    else:
        sudokus = next(
            pd.read_csv('C:/Users/srabh/Desktop/sudoku.csv', chunksize=(nb_train + nb_test))
        ).values
        
    quizzes, solutions = sudokus.T
    flatX = np.array([np.reshape([int(d) for d in flatten_grid], (9, 9))
                      for flatten_grid in quizzes])
    flaty = np.array([np.reshape([int(d) for d in flatten_grid], (9, 9))
                      for flatten_grid in solutions])
    
    return (flatX[:nb_train], flaty[:nb_train]), (flatX[nb_train:], flaty[nb_train:])


def diff(grids_true, grids_pred):
    """
    This function shows how well predicted quizzes fit to actual solutions.
    It will store sum of differences for each pair (solution, guess)
    
    Parameters
    ----------
    grids_true (np.array), shape (?, 9, 9): Real solutions to guess in the digit repesentation
    grids_pred (np.array), shape (?, 9, 9): Guesses
    
    Returns
    -------
    diff (np.array), shape (?,): Number of differences for each pair (solution, guess)
    """
    return (grids_true != grids_pred).sum((1, 2))


def delete_digits(X, n_delet=1):
    """
    This function is used to create sudoku quizzes from solutions
    
    Parameters
    ----------
    X (np.array), shape (?, 9, 9, 9|10): input solutions grids.
    n_delet (integer): max number of digit to suppress from original solutions
    
    Returns
    -------
    grids: np.array of grids to guess in one-hot way. Shape: (?, 9, 9, 10)
    """
    grids = X.argmax(3)  # get the grid in a (9, 9) integer shape
    for grid in grids:
        grid.flat[np.random.randint(0, 81, n_delet)] = 0  # generate blanks (replace = True)
        
    return to_categorical(grids)


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


# ### Prepare data for the training phase.
# 
# Since we built a `delete_digits` function, we will create our own training set from `ytrain` directly.
# Also note that we don't want targets to be (9, 9, 10) shaped. Since predicting zeros is useless ... we want to FILL zeros

# In[8]:


input_shape = (9, 9, 10)
(_, ytrain), (Xtest, ytest) = load_data()  # We won't use _. We will work directly with ytrain

# one-hot-encoding --> shapes become :
# (?, 9, 9, 10) for Xs
# (?, 9, 9, 9) for ys
Xtrain = to_categorical(ytrain).astype('float32')  # from ytrain cause we will creates quizzes from solusions
Xtest = to_categorical(Xtest).astype('float32')

ytrain = to_categorical(ytrain-1).astype('float32') # (y - 1) because we 
ytest = to_categorical(ytest-1).astype('float32')   # don't want to predict zeros


# ### Lets now define the keras model
# It consists in a simple 2 `Dense` layers and 81 `Dense` output layer (one for each digits to predict).

# In[9]:


model = Sequential()
model.add(Dense(64, activation='relu', input_shape=input_shape))
model.add(Dropout(0.4))
model.add(Dense(64, activation='relu'))
model.add(Dropout(0.4))
model.add(Flatten())

grid = Input(shape=input_shape)  # inputs
features = model(grid)  # commons features

# define one Dense layer for each of the digit we want to predict
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


# ### Train model to output solution from a solution.
# This is part of the trick. The idea comes from a workmate. Thanks to him.
# The idea is to make the network outputs the solution for the easiest quizz. That is to say ... the solution itself
# 
# ***solver(solution) &rarr; solution***
# 
# This will calibrate weights to reproduce the grid (as auto-encoders do ?).

# In[10]:


solver.fit(
    delete_digits(Xtrain, 0),  # we don't delete any digit for now
    [ytrain[:, i, j, :] for i in range(9) for j in range(9)],  # each digit of solution
    batch_size=128,
    epochs=1,  # 1 epoch should be enough for the task
    verbose=1,
)


# ### Train model to guess something harder ... and harder ... and harder. One drop at a time.
# Here we will track validation loss to avoid over fitting
# 
# First, we will pull off *a single* digit from the solution. This is quite a simple quizz to guess.
# We will train the network until it overfits
# 
# Next, lets pull off 2 of them, ... then 3, ... then 4, and so on since we reach hard enough grid with around 55 missing digits.

# In[11]:


early_stop = EarlyStopping(patience=2, verbose=1)

i = 1
for nb_epochs, nb_delete in zip(
        [1, 2, 3, 4, 6, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10],  # epochs for each round
        [1, 2, 3, 4, 6, 8, 10, 12, 15, 20, 25, 30, 35, 40, 45, 50, 55]  # digit to pull off
):
    print('Pass n° {} ...'.format(i))
    i += 1
    
    solver.fit(
        delete_digits(Xtrain, nb_delete),  # delete digits from training sample
        [ytrain[:, i, j, :] for i in range(9) for j in range(9)],
        validation_data=(
            delete_digits(Xtrain, nb_delete), # delete same amount of digit from validation sample
            [ytrain[:, i, j, :] for i in range(9) for j in range(9)]),
        batch_size=128,
        epochs=nb_epochs,
        verbose=1,
        callbacks=[early_stop]
    )


# ### Evaluate solver

# In[12]:


quizzes = Xtest.argmax(3)  # quizzes in the (?, 9, 9) shape. From the test set
true_grids = ytest.argmax(3) + 1  # true solutions dont forget to add 1 
smart_guesses = batch_smart_solve(quizzes, solver)  # make smart guesses !

deltas = diff(true_grids, smart_guesses)  # get number of errors on each quizz
accuracy = (deltas == 0).mean()  # portion of correct solved quizzes


# In[14]:


print(
"""
Grid solved:\t {}
Correct ones:\t {}
Accuracy:\t {}
""".format(
deltas.shape[0], (deltas==0).sum(), accuracy
)
)


# In[ ]:




