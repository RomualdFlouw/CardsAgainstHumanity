import Lobby from '@/components/Lobby.vue'

// Here are some Jasmine 2.0 tests, though you can
// use any test runner / assertion library combo you prefer
describe('Lobby', () => {
  // Inspect the raw component options
  it('has a created hook', () => {
    expect(typeof Lobby.created).toBe('function')
  })

  // Evaluate the results of functions in
  // the raw component options
  it('sets the correct default data', () => {
    expect(typeof Lobby.data).toBe('function')
    const defaultData = Lobby.data()
    expect(defaultData.isReady).toBe(false)
  })
})