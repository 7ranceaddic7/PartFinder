1) install ksp with rp0 and all mods you like
2) launch it at least once
3) launch partfinder
  - specify location of ksp (not game data)
  - specify a prune list (any (empty) text file) say "custom.prnl"
  - press "create nonRP0" (watch output top right)
4) go to the folder of your prune list file, there
   are three new files "custom.rp0_nocost.prnl", "custom.non_rp0.prnl" and "custom.non_ro.prnl"
   rename them (or they will be overwritten by partfinder if you do it again later)   
5) use autopruner to prune these lists
6) start ksp again

To create these prune lists again you need to unprune (everything) and start from (2)


1) install ksp with rp0 and all mods you like
2) launch ksp (unpruned) at least once (so there is a module manager cache file)
3) launch partfinder.exe
  - specify location of ksp (not game data)
  - specify a prune list (any (empty) text file) say "default.prnl"
  - press "create nonRP0"
4) [optional] go to the folder of your prune list file, there
   are three new files "default.rp0_nocost.prnl", "default.non_rp0.prnl" and "default.non_ro.prnl"
   rename them (or they will be overwritten by partfinder if you do it again later)   
5) use autopruner to prune "default.rp0_nocost.prnl", "default.non_rp0.prnl" and "default.non_ro.prnl"
   and the prunelists like RO_Global_Tanks and SXT_Tanks to get as much parts out of the way from beginning
   
6) (launch partfinder.exe)
  - specify a new prune list, say "custom.prnl"
      there are three options:
        - by title (do not uncheck wip)
        - no mods: if true it will not show part/.cfg-files created with @PART
            (recommended: unchecked)
        - parts only: it will only present you content of files where a PART tag was found 
            (recommended: checked)
  - search parts you want to remove (there is a search bar ("search"/press return))
    doupleclick on them to (or use the buttons >prune> <unprune<)
    you can rightclick on items to get information
  - press the button "resource lists"
7) use autopruner to 
  - prune custom.prnl
  - [optional] prune custom.res.prune.prnl (removes a lot of non-.cfg files)
  - [optional] unprune custom.res.unprune.prnl 
      (makes shure there are no unreferenced resources aka. black textures)
6) start ksp again


To create the non-rp0/...etc prune lists again you need to unprune (everything) and start from (2)