namespace ijw.Maths.Functions {
    public interface IFunction<TInput, TOutput> {
        TOutput Calculate(TInput input);
    }
}
