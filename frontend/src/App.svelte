<script>
    import { onMount } from "svelte";
    import auth from "./auth0Service";


  
    onMount(async () => {
      await auth.createClient();
 
      console.log(await auth.getAccessToken())
      
    });
  
    function login() {
      auth.loginWithPopup();
    }
  
    function logout() {
      auth.logout();
    }

    let {isAuthenticated, user} = auth;
  
  
  </script>

<main>
    <!-- App Bar -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
      <div class="collapse navbar-collapse" id="navbarText">
        <div class="navbar-nav mr-auto user-details">
          {#if $isAuthenticated}
          <span class="text-white">&nbsp;&nbsp;{$user.name} ({$user.email})</span>
          {:else}<span>&nbsp;</span>{/if}
        </div>
        <span class="navbar-text">
          <ul class="navbar-nav float-right">
            {#if $isAuthenticated}
            <li class="nav-item">
              <a class="nav-link" href="/#" on:click="{logout}">Log Out</a>
            </li>
            {:else}
            <li class="nav-item">
              <a class="nav-link" href="/#" on:click="{login}">Log In</a>
            </li>
            {/if}
          </ul>
        </span>
      </div>
    </nav>
  </main>