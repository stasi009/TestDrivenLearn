#include <array>
#include <iterator>
#include "gtest/gtest.h"

/*
An array, is similar to a vector except that it is of a fixed size; 
it cannot grow or shrink in size. 
The purpose of a fixed size is to allow an array to be allocated on the stack, rather than always demanding heap access as vector does. 
Just like vectors, arrays support random-access iterators, and elements are stored in contiguous memory.
*/

TEST(ArrayTest, Construct)
{
    std::array<int, 3> a1 = {6, 8, 9};
    ASSERT_EQ(a1.size(), 3);
}

TEST(ArrayTest, Index)
{
    std::array<int, 3> a1 = {6, 8, 9};
    ASSERT_EQ(a1[2], 9);
}

TEST(ArrayTest, Fill)
{
    std::array<int, 3> a1 = {6, 8, 9};
    a1.fill(99);
    ASSERT_EQ(a1, (std::array<int,3>{99, 99, 99}));
}