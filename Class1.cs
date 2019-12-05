using System;

public class Class1
{
	public Class1()
	{

        IEnumerator deathParticleSys()
        {
            //yield return new WaitForSeconds(0.3f);
            if (gameObject.name == "gritPit_long")
            {
                var particleSystemEN = gameObject.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem par in particleSystemEN)
                {
                    var em = par.emission;
                    em.enabled = true;
                    par.Play();
                }
                yield return new WaitForSeconds(2f);
                foreach (ParticleSystem par in particleSystemEN)
                {
                    var em = par.emission;
                    em.enabled = false;
                    par.Stop();
                }

            }
            else
            {
                var particleSystemEN = gameObject.GetComponentInChildren<ParticleSystem>();
                var em = particleSystemEN.emission;
                em.enabled = true;
                particleSystemEN.Play();
                yield return new WaitForSeconds(2f);
                em.enabled = false;
                particleSystemEN.Stop();
            }
            /*var particleSystemEN = gameObject.GetComponentInChildren<ParticleSystem>();
            var em = particleSystemEN.emission;
            em.enabled = true;
            particleSystemEN.Play();
            yield return new WaitForSeconds(2f);
            em.enabled = false;
            particleSystemEN.Stop(); */

        }
    }
}
