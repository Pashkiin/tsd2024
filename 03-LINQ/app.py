import random

class RandomizedList:
    def __init__(self):
        self._data = []

    def add(self, element):
        if random.choice([True, False]):
            self._data.append(element)
        else:
            self._data.insert(0, element)

    def get(self, index):
        return self._data[random.randint(0, min(index, len(self._data) - 1))]

    def is_empty(self):
        return len(self._data) == 0

# Example usage:
if __name__ == "__main__":
    rand_list = RandomizedList()
    rand_list.add(10)
    rand_list.add(20)
    rand_list.add(30)
    
    print("Random element:", rand_list.get(2))
    print("Is list empty?", rand_list.is_empty()) 

# python is dynamicallly typed language, this can lead to errors during the execution that may be hard to spot
# python is flexible so we can store any type of data in the list
# python is not type safe, so we can add any type of data in the list