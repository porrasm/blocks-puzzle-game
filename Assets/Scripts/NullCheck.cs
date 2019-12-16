public static class NullCheck {
    public static void Check(params object[] objs) {
        for(int i = 0; i < objs.Length; i++) {
            if (objs[i] == null) {
                Logger.LogError("Item " + i + " of " + objs.Length + " items was null");
            }
        }
    }
}
