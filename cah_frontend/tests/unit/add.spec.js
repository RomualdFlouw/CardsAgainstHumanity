// Function we want to test
const add = (x, y) => x + y;

// Unit test
test("should add two numbers", () => {
  const result = add(2, 3);
  expect(result).toBe(5);
});