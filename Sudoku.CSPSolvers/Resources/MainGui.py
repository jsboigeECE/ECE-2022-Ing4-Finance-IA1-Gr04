from timeit import default_timer as timer
from tkinter import messagebox

from sudokucsp import SudokuCSP
from csp import backtracking_search, mrv, unordered_domain_values, forward_checking, mac, no_inference


def solve_sudoku(self):

        s = SudokuCSP(self.current_board)
        inf, dv, suv = forward_checking, None, None

        #dans le code Steven il a un menu avec plusieurs formes de résolutions 
        ''' if self.inference.get() == "NO_INFERENCE":
            inf = no_inference
        elif self.inference.get() == "FC":
            inf = forward_checking
        elif self.inference.get() == "MAC":
            inf = mac

        if self.var_to_choose.get() == "MRV":
            suv = mrv
        '''
       

        start = timer()
        a = backtracking_search(s, select_unassigned_variable=suv, order_domain_values=unordered_domain_values,
                                inference=inf)
        end = timer()
        # if a isn't null we found a solution so we will show it in the current board
        # if a is null then we send a message to the user that the initial board
        # breaks some constraints
        if a:
            for i in range(9):
                for j in range(9):
                    index = i * 9 + j
                    self.current_board[i][j] = a.get("CELL" + str(index))
        else:
            messagebox.showerror("Error", "Invalid sudoku puzzle, please check the initial state")

        # showing solution
        self.time.set("Time: "+str(round(end-start, 5))+" seconds")
        self.n_bt.set("N. BR: "+str(s.n_bt))